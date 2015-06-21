Public Class DataLayer

    Public SqlConn As SqlConnection
    Public SqlCmd As SqlCommand
    Public SqlAdapter As SqlDataAdapter
    Public SqlDataSet As DataSet
    Public SqlDataTable As DataTable
    Public SqlReader As SqlDataReader
    Public SqlParam As SqlParameter

    Public Function SqlConnStr() As String

        Dim str As String = ConfigurationManager.ConnectionStrings("DB_LOCAL").ConnectionString.ToString()
        'Dim str As String = ConfigurationManager.ConnectionStrings("DB_SERVER_INTERNAL").ConnectionString.ToString()

        Return str

    End Function

    Public Sub SqlConnect()

        SqlConn = New SqlConnection(SqlConnStr())
        SqlConn.Open()

    End Sub

    Public Sub SqlNewCommand(ByVal cmdText As String, ByVal cmdType As String)

        SqlCmd = New SqlCommand()
        SqlCmd.Connection = SqlConn
        SqlCmd.CommandText = cmdText
        SqlCmd.CommandTimeout = 60 'in seconds

        Select Case cmdType
            Case "sp"
                SqlCmd.CommandType = CommandType.StoredProcedure
                Exit Select
            Case "text"
                SqlCmd.CommandType = CommandType.Text
                Exit Select
            Case Else
                SqlCmd.CommandType = CommandType.StoredProcedure
                Exit Select
        End Select

    End Sub

    Public Sub SqlNewAdapter(ByVal cmd As SqlCommand)

        SqlAdapter = New SqlDataAdapter(cmd)

    End Sub

    Public Sub SqlNewDataSet()

        SqlDataSet = New DataSet()

    End Sub

    Public Sub SqlFillDataTable()

        SqlDataTable = New DataTable()
        SqlAdapter.Fill(SqlDataTable)

    End Sub

    Public Sub SqlNewParam(ByVal direction As String, ByVal paramName As String, ByVal fieldValue As Object, ByVal dbType As SqlDbType, ByVal fieldLength As Integer)

        SqlParam = New SqlParameter()

        If Not fieldLength = 0 Then
            SqlParam = SqlCmd.Parameters.Add(paramName, dbType, fieldLength)
        Else
            SqlParam = SqlCmd.Parameters.Add(paramName, dbType)
        End If

        Select Case direction
            Case "Input"
                SqlParam.Direction = ParameterDirection.Input
                If (fieldValue IsNot Nothing) Then 'Allows json to send [null] values.
                    If Convert.ToString(fieldValue).Length > 0 Then 'Treats empty strings as [null].
                        Select Case dbType
                            Case SqlDbType.Int
                                SqlParam.Value = Convert.ToInt32(fieldValue)
                            Case SqlDbType.SmallInt, SqlDbType.TinyInt
                                SqlParam.Value = Convert.ToInt16(fieldValue)
                            Case SqlDbType.Bit
                                SqlParam.Value = Convert.ToByte(CorrectBoolean(fieldValue))
                            Case Else
                                SqlParam.Value = Convert.ToString(fieldValue)
                        End Select
                    Else
                        SqlParam.Value = DBNull.Value
                    End If
                Else
                    SqlParam.Value = DBNull.Value
                End If
                
            Case "Output", "InputOutput"
                SqlParam.Direction = ParameterDirection.Output
            Case "ReturnValue"
                SqlParam.Direction = ParameterDirection.ReturnValue
        End Select

    End Sub

    Public Function SqlOutputParamValue(ByVal paramName As String) As Object

        Return SqlCmd.Parameters(paramName).Value

    End Function

    Public Sub SqlExecuteCommand()

        SqlCmd.ExecuteNonQuery()

    End Sub

    Public Sub SqlExecuteReader()

        SqlReader = SqlCmd.ExecuteReader()

    End Sub

    Public Function SqlReaderHasRows() As Boolean

        SqlReaderHasRows = False
        If SqlReader.HasRows() Then
            SqlReaderHasRows = True
        End If

        Return SqlReaderHasRows

    End Function

    Public Function SqlReaderRead() As Boolean

        Return SqlReader.Read()

    End Function

    Public Function SqlReaderItem(ByVal columnName As String) As String

        Return Convert.ToString(SqlReader.Item(columnName))

    End Function

    Public Sub SqlCloseReader()

        SqlReader.Close()

    End Sub

    Public Sub SqlDisconnect()

        If SqlConn IsNot Nothing Then
            If (SqlConn.State = ConnectionState.Open) Then

                If SqlCmd.Parameters IsNot Nothing Then
                    SqlCmd.Parameters.Clear()
                End If

                If SqlCmd IsNot Nothing Then
                    SqlCmd.Dispose()
                End If

                If SqlReader IsNot Nothing Then
                    If Not (SqlReader.IsClosed) Then
                        SqlReader.Close()
                    End If
                End If

                SqlConn.Close()

            End If
            SqlConn.Dispose()
        End If

    End Sub

    Public Shared Function CorrectBoolean(value As Object) As Boolean

        CorrectBoolean = (value = "1" Or value = 1)
        Return CorrectBoolean

    End Function

End Class