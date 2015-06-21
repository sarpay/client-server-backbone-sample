' If you do not need this Web Service to be called from script, using ajax / xhr, comment the following line.
<ScriptService()> _
<WebService(Namespace:="http://api.wppetitions.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class Service : Inherits WebService

    Protected MyDataProvider As DataLayer = New DataLayer()

    <WebMethod()> _
    Public Function NewSignature( _
            ByVal site_id As Object, _
            ByVal petition_id As Object, _
            ByVal email As Object, _
            ByVal first_name As Object, _
            ByVal last_name As Object, _
            ByVal opt_in As Object, _
            ByVal address As Object, _
            ByVal city As Object, _
            ByVal state As Object, _
            ByVal zipcode As Object, _
            ByVal country As Object, _
            ByVal age_group As Object, _
            ByVal gender As Object, _
            ByVal political_id As Object _
        ) As Object()

        'Threading.Thread.Sleep(3000)

        Dim list As New List(Of Object)()
        Dim dict As New Dictionary(Of String, Object)()

        Try
            ' query the database
            MyDataProvider.SqlConnect()

            '*** Create timer
            MyDataProvider.SqlNewCommand("dbo.newSignature", "sp")

            ' INs
            MyDataProvider.SqlNewParam("Input", "@SiteID", site_id, SqlDbType.Int, 0)
            MyDataProvider.SqlNewParam("Input", "@PetID", petition_id, SqlDbType.Int, 0)
            MyDataProvider.SqlNewParam("Input", "@Email", email, SqlDbType.VarChar, 255)
            MyDataProvider.SqlNewParam("Input", "@FirstName", first_name, SqlDbType.NVarChar, 50)
            MyDataProvider.SqlNewParam("Input", "@LastName", last_name, SqlDbType.NVarChar, 50)
            MyDataProvider.SqlNewParam("Input", "@OptIn", opt_in, SqlDbType.Bit, 0)
            MyDataProvider.SqlNewParam("Input", "@Address", address, SqlDbType.NVarChar, 255)
            MyDataProvider.SqlNewParam("Input", "@City", city, SqlDbType.NVarChar, 50)
            MyDataProvider.SqlNewParam("Input", "@State", state, SqlDbType.NVarChar, 50)
            MyDataProvider.SqlNewParam("Input", "@ZipCode", zipcode, SqlDbType.NVarChar, 10)
            MyDataProvider.SqlNewParam("Input", "@Country", country, SqlDbType.NVarChar, 50)
            MyDataProvider.SqlNewParam("Input", "@AgeGroup", age_group, SqlDbType.TinyInt, 0)
            MyDataProvider.SqlNewParam("Input", "@Gender", gender, SqlDbType.TinyInt, 0)
            MyDataProvider.SqlNewParam("Input", "@PoliticalId", political_id, SqlDbType.TinyInt, 0)

            ' OUTs
            MyDataProvider.SqlNewParam("Output", "@SignID", Nothing, SqlDbType.Int, 0)

            ' Execute
            MyDataProvider.SqlExecuteCommand()

            'Get Output Value(s)
            Dim signId As Integer = Convert.ToInt32(MyDataProvider.SqlOutputParamValue("@SignID"))

            dict.Add("SignID", signId)
            list.Add(dict)

        Catch sqlEx As SqlException
            dict.Add("Error", sqlEx.ToString())
            list.Add(dict)

        Catch ex As Exception
            dict.Add("Error", ex.ToString())
            list.Add(dict)

        Finally
            MyDataProvider.SqlDisconnect()

        End Try

        Return list.ToArray()

    End Function

    <WebMethod()> _
    Public Function NewSite( _
            ByVal site_domain As Object, _
            ByVal site_title As Object, _
            ByVal admin_email As Object _
        ) As Object()

        'Threading.Thread.Sleep(3000)

        Dim list As New List(Of Object)()
        Dim dict As New Dictionary(Of String, Object)()

        Try
            ' query the database
            MyDataProvider.SqlConnect()

            '*** Create timer
            MyDataProvider.SqlNewCommand("dbo.newSite", "sp")

            ' INs
            MyDataProvider.SqlNewParam("Input", "@Domain", site_domain, SqlDbType.VarChar, 255)
            MyDataProvider.SqlNewParam("Input", "@Title", site_title, SqlDbType.NVarChar, 255)
            MyDataProvider.SqlNewParam("Input", "@Email", admin_email, SqlDbType.VarChar, 255)

            ' OUTs
            MyDataProvider.SqlNewParam("Output", "@SiteID", Nothing, SqlDbType.Int, 0)

            ' Execute
            MyDataProvider.SqlExecuteCommand()

            'Get Output Value(s)
            Dim siteId As Integer = Convert.ToInt32(MyDataProvider.SqlOutputParamValue("@SiteID"))

            dict.Add("SiteID", siteId)
            list.Add(dict)

        Catch sqlEx As SqlException
            dict.Add("Error", sqlEx.ToString())
            list.Add(dict)

        Catch ex As Exception
            dict.Add("Error", ex.ToString())
            list.Add(dict)

        Finally
            MyDataProvider.SqlDisconnect()

        End Try

        Return list.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetSignatures( _
        ) As Object()

        'Threading.Thread.Sleep(5000)

        Dim list As New List(Of Object)()
        Dim dict As New Dictionary(Of String, Object)()

        Try
            ' query the database
            MyDataProvider.SqlConnect()

            '*** Get [Users] table
            MyDataProvider.SqlNewCommand("dbo.getSignatures", "sp")
            MyDataProvider.SqlNewAdapter(MyDataProvider.SqlCmd)
            MyDataProvider.SqlFillDataTable()
            list = DataTableToList(MyDataProvider.SqlDataTable)

        Catch sqlEx As SqlException
            dict.Add("Error", sqlEx.ToString())
            list.Add(dict)

        Catch ex As Exception
            dict.Add("Error", ex.ToString())
            list.Add(dict)

        Finally
            MyDataProvider.SqlDisconnect()

        End Try

        Return list.ToArray()

    End Function

    <WebMethod()> _
    Public Function GetSites( _
        ) As Object()

        'Threading.Thread.Sleep(5000)

        Dim list As New List(Of Object)()
        Dim dict As New Dictionary(Of String, Object)()

        Try
            ' query the database
            MyDataProvider.SqlConnect()

            '*** Get [Users] table
            MyDataProvider.SqlNewCommand("dbo.getSites", "sp")
            MyDataProvider.SqlNewAdapter(MyDataProvider.SqlCmd)
            MyDataProvider.SqlFillDataTable()
            list = DataTableToList(MyDataProvider.SqlDataTable)

        Catch sqlEx As SqlException
            dict.Add("Error", sqlEx.ToString())
            list.Add(dict)

        Catch ex As Exception
            dict.Add("Error", ex.ToString())
            list.Add(dict)

        Finally
            MyDataProvider.SqlDisconnect()

        End Try

        Return list.ToArray()

    End Function

    Private Shared Function DataTableToList(dt As DataTable) As List(Of Object)

        'return dt;
        Dim rowList As New List(Of Object)()

        For Each dr As DataRow In dt.Rows
            Dim colList As New Dictionary(Of String, Object)()
            For Each dc As DataColumn In dt.Columns
                colList.Add(dc.ColumnName, If((String.Empty = dr(dc).ToString()), "", dr(dc).ToString()))
            Next
            rowList.Add(colList)
        Next

        Return rowList

    End Function

End Class