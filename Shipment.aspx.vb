#Region "Imports"

Imports System.Data.SqlClient
Imports System.Security.Principal
Imports System.Web.UI.Page
Imports System.IO

#End Region

Partial Class Shipment
    Inherits System.Web.UI.Page

    Dim pubUser As String
    Dim pubUserAccess As String
    Dim pubMaterial As Double = 0
    Dim pubCustom As Double = 0
    Dim pubFreight As Double = 0
    Dim pubInsurance As Double = 0
    Dim pubVariance As Double = 0
    Dim pubVariance2 As Double = 0

    Dim pubStandard As Double = 0

    Dim pubEndBalMat As Double = 0
    Dim pubEndBalHandling As Double = 0

    Dim pubHandling As Double = 0

    Dim pubEndBal As Double = 0

    Dim pubCounter As Integer = 0
    Dim pubCounterRec As Integer = 0

    Dim pubMaterialDebit As Double
    Dim pubMaterialCredit As Double

    Dim pubCustomDebit As Double
    Dim pubCustomCredit As Double

    Dim pubFreightDebit As Double
    Dim pubFreightCredit As Double

    Dim pubInsuranceDebit As Double
    Dim pubInsuranceCredit As Double

    Dim pubVarianceDebit As Double
    Dim pubVarianceCredit As Double

    Dim pubStandardDebit As Double
    Dim pubStandardCredit As Double

    Dim pubShipmentNo As String
    Dim pubEndBalance As Double
    Dim pubEndBalance2 As Double
    Dim pubEndBalance3 As Double

    Dim gYear As String
    Dim gPeriod As String
    Dim pubKeyValue As String
    Dim pubServerDate As String
    Dim pubSalesOrder As String

    Dim pubProjectCode As String
    Dim pubProductGroup As String

    Dim pubProjectStatus As String
    Dim pubProjectType As String
    Dim pubProjectCc As String
    Dim pubPostingCode As String
    Dim pubPostStatus As String
    Dim pubDescription As String

    Dim pubTransAmt As Double
    Dim pubTransAmt2 As Double
    Dim pubTransAmt3 As Double


    Dim pubShipmentAmt As Double
    Dim pubShipmentAmt2 As Double
    Dim pubShipmentAmt3 As Double

    Dim pubPrjxmPageNum As String
    Dim pubNljrnmPageNum As String
    Dim pubTotalDebit As Double = 0

    Dim pubTotalCredit As Double = 0
    Dim pubTotalCredit2 As Double = 0
    Dim pubTotalCredit3 As Double = 0

    Dim pubExpenseCode As String

    Dim pubAnalysisCode1 As String
    Dim pubCategory As String
    Dim pubCatAmount As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sUser() As String = Split(User.Identity.Name, "\")
        Dim sDomain As String = sUser(0)
        Dim sUserId As String = sUser(1)

        pubUser = UCase(sUserId)

        CheckPubUser()

        If Not IsPostBack Then
            If pubUserAccess = "1" Then
                MultiView1.ActiveViewIndex = 1
            Else
                Button1.Visible = False
                Button2.Visible = False
                Button4.Visible = False
                Button7.Visible = False
                Button8.Visible = False
                Button9.Visible = False

                MultiView1.ActiveViewIndex = 0

            End If
            CountPORecords()
            UpdateStatusToOpen()
            UpdateStatusToClosed()
            UpdateStatusToError()
            UpdateSalesOrder()
            UpdateTagWithError()
            'UpdateStatusToHistory()
        End If

    End Sub

    Protected Sub GridView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.PageIndexChanged

        If GridView1.PageIndex + 1 = Val(Label15.Text) Then
            SummPOShipment()
            GridView1.ShowFooter = True
        Else
            GridView1.ShowFooter = False
        End If

    End Sub

    Protected Sub GridView2_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.PageIndexChanged
        If GridView2.PageIndex + 1 = Val(Label15.Text) Then
            SummPOShipment()
            GridView2.ShowFooter = True
        Else
            GridView2.ShowFooter = False
        End If

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        Select Case e.CommandName

            Case "Select"

                Dim rowindex As Integer = CInt(e.CommandArgument)

                Label1.Text = rowindex.ToString.Trim

                Button1.Visible = False
                Button2.Visible = False
                Button4.Visible = False
                Button6.Visible = False
                Button7.Visible = False
                Button8.Visible = False
                Button9.Visible = False


                Label9.Visible = False
                Label10.Visible = False
                Label11.Visible = False
                Label12.Visible = False
                Label13.Visible = False
                Label14.Visible = False

                DisplayLabels()

                MultiView1.ActiveViewIndex = 2

        End Select

    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand


        Select Case e.CommandName

            Case "Select"
                Try

                    Dim rowindex As Integer = CInt(e.CommandArgument)
                    'Dim row As GridViewRow = GridView2.Rows(rowindex)

                    Label1.Text = rowindex.ToString.Trim

                    HideAndDisplay()

                Catch ex As Exception
                    Label1.Text = e.CommandArgument
                    HideAndDisplay()
                End Try

        End Select

    End Sub

    Private Sub HideAndDisplay()

        Button1.Visible = False
        Button2.Visible = False
        Button4.Visible = False
        Button6.Visible = False
        Button7.Visible = False
        Button8.Visible = False
        Button9.Visible = False

        Label9.Visible = False
        Label10.Visible = False
        Label11.Visible = False
        Label12.Visible = False
        Label13.Visible = False
        Label14.Visible = False

        DisplayLabels()

        MultiView1.ActiveViewIndex = 2

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "mat_cost")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "mat_cost")) = 0 Then
                    e.Row.Cells(1).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance")) = 0 Then
                    e.Row.Cells(2).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "standard")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "standard")) = 0 Then
                    e.Row.Cells(3).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "end_bal_material")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "end_bal_material")) = 0 Then
                    e.Row.Cells(4).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "customs")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "customs")) = 0 Then
                    e.Row.Cells(5).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "freight")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "freight")) = 0 Then
                    e.Row.Cells(6).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "insurance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "insurance")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance2")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance2")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "handling")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "handling")) = 0 Then
                    e.Row.Cells(9).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "end_bal_handling")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "end_bal_handling")) = 0 Then
                    e.Row.Cells(10).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "endbal")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "endbal")) = 0 Then
                    e.Row.Cells(11).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total :"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubMaterial < 0 Then
                e.Row.Cells(1).Text = "(" & Mid(pubMaterial.ToString("c"), 3, Len(pubMaterial.ToString("c")) - 2)
                e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(1).Text = Mid(pubMaterial.ToString("c"), 2, Len(pubMaterial.ToString("c")) - 1)
                e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubVariance < 0 Then
                e.Row.Cells(2).Text = "(" & Mid(pubVariance.ToString("c"), 3, Len(pubVariance.ToString("c")) - 2)
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(2).Text = Mid(pubVariance.ToString("c"), 2, Len(pubVariance.ToString("c")) - 1)
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubStandard < 0 Then
                e.Row.Cells(3).Text = "(" & Mid(pubStandard.ToString("c"), 3, Len(pubStandard.ToString("c")) - 2)
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(3).Text = Mid(pubStandard.ToString("c"), 2, Len(pubStandard.ToString("c")) - 1)
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBalMat < 0 Then
                e.Row.Cells(4).Text = "(" & Mid(pubEndBalMat.ToString("c"), 3, Len(pubEndBalMat.ToString("c")) - 2)
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(4).Text = Mid(pubEndBalMat.ToString("c"), 2, Len(pubEndBalMat.ToString("c")) - 1)
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubCustom < 0 Then
                e.Row.Cells(5).Text = "(" & Mid(pubCustom.ToString("c"), 3, Len(pubCustom.ToString("c")) - 2)
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(5).Text = Mid(pubCustom.ToString("c"), 2, Len(pubCustom.ToString("c")) - 1)
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubFreight < 0 Then
                e.Row.Cells(6).Text = "(" & Mid(pubFreight.ToString("c"), 3, Len(pubFreight.ToString("c")) - 2)
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(6).Text = Mid(pubFreight.ToString("c"), 2, Len(pubFreight.ToString("c")) - 1)
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubInsurance < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubInsurance.ToString("c"), 3, Len(pubInsurance.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubInsurance.ToString("c"), 2, Len(pubInsurance.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubVariance2 < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubVariance2.ToString("c"), 3, Len(pubVariance2.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubVariance2.ToString("c"), 2, Len(pubVariance2.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubHandling < 0 Then
                e.Row.Cells(9).Text = "(" & Mid(pubHandling.ToString("c"), 3, Len(pubHandling.ToString("c")) - 2)
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(9).Text = Mid(pubHandling.ToString("c"), 2, Len(pubHandling.ToString("c")) - 1)
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBalHandling < 0 Then
                e.Row.Cells(10).Text = "(" & Mid(pubEndBalHandling.ToString("c"), 3, Len(pubEndBalHandling.ToString("c")) - 2)
                e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(10).Text = Mid(pubEndBalHandling.ToString("c"), 2, Len(pubEndBalHandling.ToString("c")) - 1)
                e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBal < 0 Then
                e.Row.Cells(11).Text = "(" & Mid(pubEndBal.ToString("c"), 3, Len(pubEndBal.ToString("c")) - 2)
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(11).Text = Mid(pubEndBal.ToString("c"), 2, Len(pubEndBal.ToString("c")) - 1)
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            End If

        End If
    End Sub

    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "mat_cost")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "mat_cost")) = 0 Then
                    e.Row.Cells(2).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance")) = 0 Then
                    e.Row.Cells(3).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "standard")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "standard")) = 0 Then
                    e.Row.Cells(4).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "end_bal_material")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "end_bal_material")) = 0 Then
                    e.Row.Cells(5).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "customs")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "customs")) = 0 Then
                    e.Row.Cells(6).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "freight")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "freight")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "insurance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "insurance")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance2")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance2")) = 0 Then
                    e.Row.Cells(9).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "handling")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "handling")) = 0 Then
                    e.Row.Cells(10).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "end_bal_handling")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "end_bal_handling")) = 0 Then
                    e.Row.Cells(11).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "endbal")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "endbal")) = 0 Then
                    e.Row.Cells(12).Text = ""
                End If
            End If


            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "status")) Then
                If DataBinder.Eval(e.Row.DataItem, "status").ToString.Trim = "Error" Then
                    'e.Row.Cells(0).Enabled = False
                    e.Row.Cells(0).BackColor = Drawing.Color.Red
                    e.Row.Cells(1).BackColor = Drawing.Color.Red
                    e.Row.Cells(2).BackColor = Drawing.Color.Red
                    e.Row.Cells(3).BackColor = Drawing.Color.Red
                    e.Row.Cells(4).BackColor = Drawing.Color.Red
                    e.Row.Cells(5).BackColor = Drawing.Color.Red
                    e.Row.Cells(6).BackColor = Drawing.Color.Red
                    e.Row.Cells(7).BackColor = Drawing.Color.Red
                    e.Row.Cells(8).BackColor = Drawing.Color.Red
                    e.Row.Cells(9).BackColor = Drawing.Color.Red
                    e.Row.Cells(10).BackColor = Drawing.Color.Red
                    e.Row.Cells(11).BackColor = Drawing.Color.Red
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "tag")) Then
                If Convert.ToChar(DataBinder.Eval(e.Row.DataItem, "tag")).ToString.Trim = "V" Then
                    e.Row.Cells(0).Enabled = False
                    e.Row.Cells(0).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(1).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(2).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(3).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(4).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(5).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(6).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(7).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(8).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(9).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(10).BackColor = Drawing.Color.HotPink
                    e.Row.Cells(11).BackColor = Drawing.Color.HotPink
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = "Total :"
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Center

            ' for the Footer, display the running totals
            If pubMaterial < 0 Then
                e.Row.Cells(2).Text = "(" & Mid(pubMaterial.ToString("c"), 3, Len(pubMaterial.ToString("c")) - 2)
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(2).Text = Mid(pubMaterial.ToString("c"), 2, Len(pubMaterial.ToString("c")) - 1)
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubVariance < 0 Then
                e.Row.Cells(3).Text = "(" & Mid(pubVariance.ToString("c"), 3, Len(pubVariance.ToString("c")) - 2)
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(3).Text = Mid(pubVariance.ToString("c"), 2, Len(pubVariance.ToString("c")) - 1)
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubStandard < 0 Then
                e.Row.Cells(4).Text = "(" & Mid(pubStandard.ToString("c"), 3, Len(pubStandard.ToString("c")) - 2)
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(4).Text = Mid(pubStandard.ToString("c"), 2, Len(pubStandard.ToString("c")) - 1)
                e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBalMat < 0 Then
                e.Row.Cells(5).Text = "(" & Mid(pubEndBalMat.ToString("c"), 3, Len(pubEndBalMat.ToString("c")) - 2)
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(5).Text = Mid(pubEndBalMat.ToString("c"), 2, Len(pubEndBalMat.ToString("c")) - 1)
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubCustom < 0 Then
                e.Row.Cells(6).Text = "(" & Mid(pubCustom.ToString("c"), 3, Len(pubCustom.ToString("c")) - 2)
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(6).Text = Mid(pubCustom.ToString("c"), 2, Len(pubCustom.ToString("c")) - 1)
                e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubFreight < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubFreight.ToString("c"), 3, Len(pubFreight.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubFreight.ToString("c"), 2, Len(pubFreight.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubInsurance < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubInsurance.ToString("c"), 3, Len(pubInsurance.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubInsurance.ToString("c"), 2, Len(pubInsurance.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubVariance2 < 0 Then
                e.Row.Cells(9).Text = "(" & Mid(pubVariance2.ToString("c"), 3, Len(pubVariance2.ToString("c")) - 2)
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(9).Text = Mid(pubVariance2.ToString("c"), 2, Len(pubVariance2.ToString("c")) - 1)
                e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubHandling < 0 Then
                e.Row.Cells(10).Text = "(" & Mid(pubHandling.ToString("c"), 3, Len(pubHandling.ToString("c")) - 2)
                e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(10).Text = Mid(pubHandling.ToString("c"), 2, Len(pubHandling.ToString("c")) - 1)
                e.Row.Cells(10).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBalHandling < 0 Then
                e.Row.Cells(11).Text = "(" & Mid(pubEndBalHandling.ToString("c"), 3, Len(pubEndBalHandling.ToString("c")) - 2)
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(11).Text = Mid(pubEndBalHandling.ToString("c"), 2, Len(pubEndBalHandling.ToString("c")) - 1)
                e.Row.Cells(11).HorizontalAlign = HorizontalAlign.Right
            End If

            If pubEndBal < 0 Then
                e.Row.Cells(12).Text = "(" & Mid(pubEndBal.ToString("c"), 3, Len(pubEndBal.ToString("c")) - 2)
                e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(12).Text = Mid(pubEndBal.ToString("c"), 2, Len(pubEndBal.ToString("c")) - 1)
                e.Row.Cells(12).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UnCheckAllItems()
    End Sub

    Private Sub DisplayLabels()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SESLSVRHO;User ID=scheme;Password=Er1c550n2"

        Dim MySqlConn As New SqlConnection(ConnStr)

        MySqlConn.Open()
        Try

            sSql = "Select category from SES.scheme.and_poshipment_master where analysis_code1 = '" & _
                    Label1.Text.Trim & "' group by category"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    Select Case Trim(mReader("category"))
                        Case "M"
                            Label9.Visible = True
                        Case "C"
                            Label10.Visible = True
                        Case "F"
                            Label11.Visible = True
                        Case "I"
                            Label12.Visible = True
                        Case "S"
                            Label14.Visible = True
                        Case "V"
                            Label13.Visible = True
                    End Select

                End While
            End If

        Catch ex As Exception
            MySqlConn.Close()
        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub CountPORecords()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SESLSVRHO;User ID=scheme;Password=Er1c550n2"

        Dim MySqlConn As New SqlConnection(ConnStr)

        MySqlConn.Open()
        Try

            sSql = "Select count(shipment_no) as countpo from SES.scheme.and_poshipment where tag <> 'H'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()
                    If mReader("countpo") Mod 20 = 0 Then
                        Label15.Text = mReader("countpo") / 20
                    Else
                        Label15.Text = Int((mReader("countpo") / 20) + 1)
                    End If

                End While
            End If

        Catch ex As Exception
            MySqlConn.Close()
        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub SummPOShipment()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SESLSVRHO;User ID=scheme;Password=Er1c550n2"

        Dim MySqlConn As New SqlConnection(ConnStr)

        MySqlConn.Open()
        Try

            sSql = "Select sum(mat_cost) as mat_cost, sum(customs) as customs, sum(freight) as freight, sum(insurance) as insurance, " & _
                    "sum(variance) as variance, sum(variance2) as variance2, sum(standard) as standard, sum(handling) as handling," & _
                    "sum(mat_cost+variance+standard) as end_bal_material, sum(customs+freight+insurance+variance2+handling) as end_bal_handling," & _
                    "sum((mat_cost+variance+standard)+(customs+freight+insurance+variance2+handling)) as endbal from SES.scheme.and_poshipment where tag <> 'H'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    pubMaterial = mReader("mat_cost")
                    pubCustom = mReader("customs")
                    pubFreight = mReader("freight")
                    pubInsurance = mReader("insurance")
                    pubVariance = mReader("variance")
                    pubVariance2 = mReader("variance2")

                    pubEndBalMat = mReader("end_bal_material")
                    pubEndBalHandling = (mReader("end_bal_handling"))

                    pubStandard = mReader("standard")
                    pubHandling = mReader("handling")
                    pubEndBal = mReader("end_bal_material") + mReader("end_bal_handling")

                End While
            End If

        Catch ex As Exception
            MySqlConn.Close()
        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub UnCheckAllItems()

        Dim dr As GridViewRow
        Dim gIndex As Integer = -1
        For Each dr In GridView2.Rows

            If gIndex = -1 Then
                gIndex = 0
            End If

            Dim RowCheckBox As CheckBox = CType(GridView2.Rows(gIndex).FindControl("CheckBox1"), CheckBox)

            RowCheckBox.Checked = False

            gIndex += 1

        Next

    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If pubUserAccess = "1" Then
            Button1.Visible = True
            Button2.Visible = True
            Button4.Visible = True
            Button6.Visible = True
            Button7.Visible = True
            Button8.Visible = True
            Button9.Visible = True
            If Button8.Text.Trim = "Live" Then
                MultiView1.ActiveViewIndex = 4
            Else
                MultiView1.ActiveViewIndex = 1
            End If
        Else
            MultiView1.ActiveViewIndex = 0
            Button6.Visible = True
        End If

    End Sub

    Protected Sub GridView4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView4.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubMaterialDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubMaterialCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubMaterialDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubMaterialDebit.ToString("c"), 3, Len(pubMaterialDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubMaterialDebit.ToString("c"), 2, Len(pubMaterialDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubMaterialCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubMaterialCredit.ToString("c"), 3, Len(pubMaterialCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubMaterialCredit.ToString("c"), 2, Len(pubMaterialCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub GridView5_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView5.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubCustomDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubCustomCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubCustomDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubCustomDebit.ToString("c"), 3, Len(pubCustomDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubCustomDebit.ToString("c"), 2, Len(pubCustomDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubCustomCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubCustomCredit.ToString("c"), 3, Len(pubCustomCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubCustomCredit.ToString("c"), 2, Len(pubCustomCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub GridView6_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView6.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubFreightDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubFreightCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubFreightDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubFreightDebit.ToString("c"), 3, Len(pubFreightDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubFreightDebit.ToString("c"), 2, Len(pubFreightDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubFreightCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubFreightCredit.ToString("c"), 3, Len(pubFreightCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubFreightCredit.ToString("c"), 2, Len(pubFreightCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub GridView7_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView7.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubInsuranceDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubInsuranceCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubInsuranceDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubInsuranceDebit.ToString("c"), 3, Len(pubInsuranceDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubInsuranceDebit.ToString("c"), 2, Len(pubInsuranceDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubInsuranceCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubInsuranceCredit.ToString("c"), 3, Len(pubInsuranceCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubInsuranceCredit.ToString("c"), 2, Len(pubInsuranceCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub GridView8_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView8.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubVarianceDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubVarianceCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubVarianceDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubVarianceDebit.ToString("c"), 3, Len(pubVarianceDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubVarianceDebit.ToString("c"), 2, Len(pubVarianceDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubVarianceCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubVarianceCredit.ToString("c"), 3, Len(pubVarianceCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubVarianceCredit.ToString("c"), 2, Len(pubVarianceCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Protected Sub GridView9_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView9.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "gldebit")) Then
                pubStandardDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "gldebit")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "glcredit")) Then
                pubStandardCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit"))
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "glcredit")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = "Total :"
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Center
            ' for the Footer, display the running totals
            If pubStandardDebit < 0 Then
                e.Row.Cells(7).Text = "(" & Mid(pubStandardDebit.ToString("c"), 3, Len(pubStandardDebit.ToString("c")) - 2)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(7).Text = Mid(pubStandardDebit.ToString("c"), 2, Len(pubStandardDebit.ToString("c")) - 1)
                e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            End If


            If pubStandardCredit < 0 Then
                e.Row.Cells(8).Text = "(" & Mid(pubStandardCredit.ToString("c"), 3, Len(pubStandardCredit.ToString("c")) - 2)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            Else
                e.Row.Cells(8).Text = Mid(pubStandardCredit.ToString("c"), 2, Len(pubStandardCredit.ToString("c")) - 1)
                e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        End If

    End Sub

    Private Sub CheckPubUser()

        Dim ConnStr As String
        Dim sSql As String
        ConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select access_tag from and_poshipment_users where userid = '" & pubUser.Trim & "'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()
                    pubUserAccess = mReader("access_tag")
                End While
            Else
                Response.Redirect("MgtUnauthorized.aspx")
            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If GridView2.AllowPaging = True Then
            GridView2.AllowPaging = False
            Button4.Text = "Enable Paging"
            GridView2.ShowFooter = True
            SummPOShipment()
        Else
            GridView2.AllowPaging = True
            GridView2.PageIndex = 0
            Button4.Text = "Disable Paging"
            GridView2.ShowFooter = False
        End If
    End Sub

    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Button1.Visible = False
        Button2.Visible = False
        Button4.Visible = False
        Button6.Visible = False
        Button7.Visible = False
        Button8.Visible = False
        Button9.Visible = False
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If pubUserAccess = "1" Then
            Button1.Visible = True
            Button2.Visible = True
            Button4.Visible = True
            Button6.Visible = True
            Button7.Visible = False
            Button8.Visible = False
            Button9.Visible = False
            MultiView1.ActiveViewIndex = 1
        Else
            MultiView1.ActiveViewIndex = 0
            Button6.Visible = True
        End If
    End Sub

    Private Sub UpdateStatusToOpen()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "Update and_poshipment set status = 'Open' where shipment_no in " & _
                            "(Select a.order_no as PONumber from SAGE.cs3live.scheme.podetm a inner join " & _
                          "SAGE.cs3live.scheme.poheadm b on a.order_no=b.order_no " & _
                          "where qty_ordered-qty_received<>0 and a.status<>'R' and line_type='P' and b.status<>'9')"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateStatusToHistory()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "update and_poshipment set tag = 'H' where mat_cost+variance+standard+customs+freight+insurance+variance2+handling = 0 and tag ='C'"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateStatusToClosed()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "Update and_poshipment set status = 'Closed' where shipment_no not in " & _
                                "(Select a.order_no as PONumber from SAGE.cs3live.scheme.podetm a inner join " & _
                              "SAGE.cs3live.scheme.poheadm b on a.order_no=b.order_no " & _
                              "where qty_ordered-qty_received<>0 and a.status<>'R' and line_type='P' and b.status<>'9')"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateStatusToError()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "Update and_poshipment set status = 'Error' where shipment_no not in " & _
                                    "(Select order_no from SAGE.cs3live.scheme.poheadm)"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateSalesOrder()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "UPDATE and_poshipment set salesorder = (SELECT memo1 FROM SAGE.cs3live.scheme.poheadm AS so " & _
                                 "WHERE (order_no = (SELECT shipment_no FROM and_poshipment AS so2 WHERE (shipment_no = so.order_no))) " & _
                                 "AND (and_poshipment.shipment_no = order_no))"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Protected Sub GridView3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView3.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "mat_cost")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "mat_cost")) = 0 Then
                    e.Row.Cells(1).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "customs")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "customs")) = 0 Then
                    e.Row.Cells(2).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "freight")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "freight")) = 0 Then
                    e.Row.Cells(3).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "insurance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "insurance")) = 0 Then
                    e.Row.Cells(4).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance")) = 0 Then
                    e.Row.Cells(5).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "actual")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "actual")) = 0 Then
                    e.Row.Cells(6).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "standard")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "standard")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "endbal")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "endbal")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        CreateJournal()

        Dim dr As GridViewRow
        Dim gIndex As Integer = -1
        For Each dr In GridView2.Rows

            If gIndex = -1 Then
                gIndex = 0
            End If

            Dim RowCheckBox As CheckBox = CType(GridView2.Rows(gIndex).FindControl("CheckBox1"), CheckBox)

            If RowCheckBox.Checked = True Then
                'pubShipmentNo = GridView2.Rows(gIndex).Cells(10).Text



                pubShipmentNo = CType(GridView2.Rows(gIndex).FindControl("Linkbutton1"), LinkButton).Text


                pubSalesOrder = GridView2.Rows(gIndex).Cells(14).Text

                pubProjectCode = pubSalesOrder

                pubDescription = "P/VAR-SES " & pubShipmentNo.Trim

                If InStr(1, Trim(pubSalesOrder.Trim), "/") > 0 Then
                    pubSalesOrder = Mid(pubSalesOrder.Trim, 1, 6)
                End If

                GetEndBal()

                If pubTransAmt = 0 And pubTransAmt2 = 0 And pubTransAmt3 = 0 Then



                Else


                    If pubCounter = 10 Then
                        UpdateNljrnmNoLines()
                        pubCounter = 0
                    End If

                    pubCounter = pubCounter + 1
                    pubCounterRec = pubCounterRec + 1



                    If pubCounterRec <= 10 Then
                        pubPrjxmPageNum = "   "
                        pubNljrnmPageNum = "    "
                    Else
                        If pubCounterRec Mod 10 <> 0 Then
                            If pubCounterRec > 100 Then
                                If pubCounterRec > 1000 Then
                                    pubPrjxmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                                    pubNljrnmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                                Else
                                    pubPrjxmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                                    pubNljrnmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                                End If
                            Else
                                pubPrjxmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                                pubNljrnmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                            End If
                        End If

                    End If


                    AddJournal()
                    AddJournal1570()
                    AddJournal1572()

                    'update nljrnm
                    UpdateNljrnmLinesDebitCredit()



                    'UpdateTagPOMaster()


                    UpdateTagPOShipment()

                End If
                End If

            gIndex += 1

        Next

        InsertIntoLiveNljrnm()
        InsertIntoLiveNlanalm()
        InsertIntoLivePrjxm()

        GridView2.DataBind()

    End Sub

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        GridView2.DataBind()

    End Sub

    Private Sub UpdateNljrnmLinesDebitCredit()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec < 10 Then
            cmdUpdate.CommandText = "Update sir_nljrnm set total_no_of_lines = '" & Trim(Str(pubCounterRec)) & _
                    "   ', last_line_key = '" & Trim(Str(pubCounterRec)) & "', total_debits  = " & pubTotalCredit3 & ", total_credits = " & pubTotalCredit3 & _
                    " where page_no = '    '"
        Else
            cmdUpdate.CommandText = "Update sir_nljrnm set total_no_of_lines = '" & Trim(Str(pubCounterRec)) & _
                    "  ', last_line_key = '" & Trim(Str(pubCounterRec)) & "', total_debits  = " & pubTotalCredit3 & ", total_credits = " & pubTotalCredit3 & _
                    " where page_no = '    '"
        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateTagPOMaster()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "Update and_poshipment_master set tag = '0' where analysis_code1 = '" & pubShipmentNo.Trim & "'"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateTagPOShipment()

        Dim strConnStr As String
        strConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        'V - Post Variance H - History  C - Current
        cmdUpdate.CommandText = "Update and_poshipment set tag = 'V' where shipment_no = '" & pubShipmentNo.Trim & "'"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub CreateJournal()

        GetPeriod()
        GetUpdateJournalKey()
        GetServerDateTime()
        DeleteSirNljrnm()
        DeleteSirNlanalm()
        DeleteSirPrpjxm()

    End Sub

    Private Sub GetPeriod()

        Dim ConnStr As String
        Dim sSql As String
        ConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select system_key, key_value from sysdirm where system_key = 'NLPERIOD'" & _
                        " or system_key = 'NLYEAR'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    If Trim(mReader("system_key")) = "NLPERIOD" Then
                        gPeriod = Trim(Str(Val(Trim(mReader("key_value")))))
                    Else
                        gYear = "20" & Trim(mReader("key_value"))
                    End If

                End While

            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub GetUpdateJournalKey()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select system_key, key_value from sysdirm where system_key ='NLLASTJRN'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    pubKeyValue = Format(Val(mReader("key_value")) + 1, "000000000")


                    UpdateSystemKey()


                End While

            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub UpdateSystemKey()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "update sysdirm set key_value = '" & pubKeyValue.Trim & "' where system_key ='NLLASTJRN'"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub DeleteSirNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "delete from sir_nljrnm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub DeleteSirNlanalm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "delete from sir_nlanalm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub DeleteSirPrpjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "delete from sir_prjxm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertRecordNlanalm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec < 10 Then
            cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                "values('" & pubKeyValue & "','          ','   " & Trim(Str(pubCounterRec)) & "','" & pubProjectCode.Trim & _
                "','1415','" & pubProductGroup & "')"
        Else
            If pubCounterRec > 99 Then
                If pubCounterRec > 999 Then
                    cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                        "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ','" & Trim(Str(pubCounterRec)) & "','" & pubProjectCode.Trim & _
                        "','1415','" & pubProductGroup & "')"
                Else
                    cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                        "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ',' " & Trim(Str(pubCounterRec)) & "','" & pubProjectCode.Trim & _
                        "','1415','" & pubProductGroup & "')"
                End If
            Else
                cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                    "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ','  " & Trim(Str(pubCounterRec)) & "','" & pubProjectCode.Trim & _
                    "','1415','" & pubProductGroup & "')"
            End If
        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertShipmentNlanalm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        Select Case pubCounterRec
            Case 1 To 9
                cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                    "values('" & pubKeyValue & "','          ','   " & Trim(Str(pubCounterRec)) & "','V" & pubShipmentNo.Trim & _
                    "','1415',' ')"
            Case 10
                cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                    "values('" & pubKeyValue & "','          ','  " & Trim(Str(pubCounterRec)) & "','V" & pubShipmentNo.Trim & _
                    "','1415',' ')"
            Case Else

                If pubCounterRec > 99 Then
                    If pubCounterRec > 999 Then
                        cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                            "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ','" & Trim(Str(pubCounterRec)) & "','V" & pubShipmentNo.Trim & _
                            "','1415',' ')"
                    Else
                        cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                            "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ',' " & Trim(Str(pubCounterRec)) & "','V" & pubShipmentNo.Trim & _
                            "','1415',' ')"
                    End If
                Else
                    cmdUpdate.CommandText = "insert into sir_nlanalm(journal,page_no,line,analysis1,analysis2,analysis3) " & _
                        "values('" & pubKeyValue & "','/" & pubNljrnmPageNum.Trim & "      ','  " & Trim(Str(pubCounterRec)) & "','V" & pubShipmentNo.Trim & _
                        "','1415',' ')"
                End If

        End Select

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateShipmentPrjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounter < 10 Then
            cmdUpdate.CommandText = "update sir_prjxm set jobcode0" & Trim(Str(pubCounter)) & " = '" & Space(20) & "', expcode0" & Trim(Str(pubCounter)) & _
                    " = '" & Space(20) & "' where jrn_num = '" & pubKeyValue.Trim & "' and page_num = '" & pubPrjxmPageNum & "'"
        Else
            cmdUpdate.CommandText = "update sir_prjxm set jobcode" & Trim(Str(pubCounter)) & " = '" & Space(20) & "', expcode" & Trim(Str(pubCounter)) & _
                    " = '" & Space(20) & "' where jrn_num = '" & pubKeyValue.Trim & "' and page_num = '" & pubPrjxmPageNum & "'"
        End If


        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdatePrpjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounter < 10 Then
            cmdUpdate.CommandText = "update sir_prjxm set jobcode0" & Trim(Str(pubCounter)) & " = '" & pubSalesOrder & "', expcode0" & Trim(Str(pubCounter)) & _
                    " = '" & pubExpenseCode & "' where jrn_num = '" & pubKeyValue.Trim & "' and page_num = '" & pubPrjxmPageNum & "'"

        Else
            cmdUpdate.CommandText = "update sir_prjxm set jobcode" & Trim(Str(pubCounter)) & " = '" & pubSalesOrder & "', expcode" & Trim(Str(pubCounter)) & _
                    " = '" & pubExpenseCode & "' where jrn_num = '" & pubKeyValue.Trim & "' and page_num = '" & pubPrjxmPageNum & "'"
        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertRecordPrjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec <= 10 Then
            cmdUpdate.CommandText = "insert into sir_prjxm(jrn_num,delimiter,page_num,jobcode01,expcode01) " & _
                "values('" & pubKeyValue & "',' ','   ','" & pubSalesOrder & _
                "','" & pubExpenseCode & "')"

        Else
            cmdUpdate.CommandText = "insert into sir_prjxm(jrn_num,delimiter,page_num,jobcode01,expcode01) " & _
                "values('" & pubKeyValue & "','/','" & pubNljrnmPageNum & "','" & pubSalesOrder & _
                "','" & pubExpenseCode & "')"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertRecordNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec <= 10 Then
            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "',' ','    ','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','" & pubPostingCode.Trim & "'," & _
                pubTransAmt3 & ",'   ',0,'" & pubDescription.Trim & "','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubTransAmt3 & "," & pubTransAmt3 & ",1,' ','          ','N',0,0,1)"
        Else

            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "','/','" & pubNljrnmPageNum & "','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','" & pubPostingCode.Trim & "'," & _
                pubTransAmt3 & ",'   ',0,'" & pubDescription.Trim & "','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubTransAmt3 & "," & pubTransAmt3 & ",1,' ','          ','N',0,0,1)"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertRecordNljrnm1570()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec <= 10 Then
            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "',' ','    ','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','20-100-1-15-1570'," & _
                pubShipmentAmt & ",'   ',0,'PRICE VARIANCE','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubShipmentAmt & "," & pubShipmentAmt & ",1,' ','          ','N',0,0,1)"
        Else

            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "','/','" & pubNljrnmPageNum & "','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','20-100-1-15-1570'," & _
                pubShipmentAmt & ",'   ',0,'PRICE VARIANCE','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubShipmentAmt & "," & pubShipmentAmt & ",1,' ','          ','N',0,0,1)"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertRecordNljrnm1572()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounterRec <= 10 Then
            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "',' ','    ','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','20-100-1-15-1572'," & _
                pubShipmentAmt2 & ",'   ',0,'PRICE VARIANCE','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubShipmentAmt2 & "," & pubShipmentAmt2 & ",1,' ','          ','N',0,0,1)"
        Else

            cmdUpdate.CommandText = "insert into sir_nljrnm(journal_no,slash,page_no,dated,narrative,hold_indicator,action_indicator," & _
                "period_indicator,period_entered,total_debits,total_credits,no_of_lines,total_no_of_lines," & _
                "nominal_codes01,line_amounts01,line_vatcodes01,line_vat_amts01,line_narrative01,nlyear,transdate,source1,source2," & _
                "rept_curr_cd01,curr_code01,curr_type01,fix_rate_ind01,curr_freq01,multidiv01,revalue_status01,analysis1," & _
                "analysis2,analysis3,trans_amount01,report_amount01,exchange_rate01,interco_flag,transaction_group," & _
                "group_journal,journal_type,last_line_key,line_key01) values('" & _
                pubKeyValue & "','/','" & pubNljrnmPageNum & "','" & pubServerDate & "','MALIK',' ','   ','  ','  '," & _
                "0" & "," & "0" & ",' ','0','20-100-1-15-1572'," & _
                pubShipmentAmt2 & ",'   ',0,'PRICE VARIANCE','  ','" & pubServerDate & "','" & Space(24) & "','" & _
                Space(24) & "','SAR','SAR','YY','N','Y','*','O','          ','          ','          '," & _
                pubShipmentAmt2 & "," & pubShipmentAmt2 & ",1,' ','          ','N',0,0,1)"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounter < 10 Then
            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes0" & Trim(Str(pubCounter)) & " = '" & pubPostingCode.Trim & _
                        "', line_amounts0" & Trim(Str(pubCounter)) & " = " & pubTransAmt3 & _
                        ", line_vatcodes0" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts0" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative0" & Trim(Str(pubCounter)) & " = '" & pubDescription.Trim & _
                        "', rept_curr_cd0" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code0" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type0" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind0" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq0" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv0" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status0" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount0" & Trim(Str(pubCounter)) & "= " & pubTransAmt3 & _
                        ",report_amount0" & Trim(Str(pubCounter)) & "= " & pubTransAmt3 & _
                        ",exchange_rate0" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key0" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"
        Else

            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes" & Trim(Str(pubCounter)) & " = '" & pubPostingCode.Trim & _
                        "', line_amounts" & Trim(Str(pubCounter)) & " = " & pubTransAmt3 & _
                        ", line_vatcodes" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative" & Trim(Str(pubCounter)) & " = '" & pubDescription.Trim & _
                        "', rept_curr_cd" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount" & Trim(Str(pubCounter)) & "= " & pubTransAmt3 & _
                        ",report_amount" & Trim(Str(pubCounter)) & "= " & pubTransAmt3 & _
                        ",exchange_rate" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateShipmentNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand()


        If pubCounter = 10 Then


            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes" & Trim(Str(pubCounter)) & " = '20-100-1-15-1570" & _
                        "', line_amounts" & Trim(Str(pubCounter)) & " = " & pubShipmentAmt & _
                        ", line_vatcodes" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative" & Trim(Str(pubCounter)) & " = 'PRICE VARIANCE" & _
                        "', rept_curr_cd" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt & _
                        ",report_amount" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt & _
                        ",exchange_rate" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"

        Else

            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes0" & Trim(Str(pubCounter)) & " = '20-100-1-15-1570" & _
                        "', line_amounts0" & Trim(Str(pubCounter)) & " = " & pubShipmentAmt & _
                        ", line_vatcodes0" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts0" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative0" & Trim(Str(pubCounter)) & " = 'PRICE VARIANCE" & _
                        "', rept_curr_cd0" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code0" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type0" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind0" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq0" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv0" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status0" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount0" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt & _
                        ",report_amount0" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt & _
                        ",exchange_rate0" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key0" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateShipmentNljrnm2()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand()


        If pubCounter = 10 Then


            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes" & Trim(Str(pubCounter)) & " = '20-100-1-15-1572" & _
                        "', line_amounts" & Trim(Str(pubCounter)) & " = " & pubShipmentAmt2 & _
                        ", line_vatcodes" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative" & Trim(Str(pubCounter)) & " = 'PRICE VARIANCE" & _
                        "', rept_curr_cd" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt2 & _
                        ",report_amount" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt2 & _
                        ",exchange_rate" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"

        Else

            cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes0" & Trim(Str(pubCounter)) & " = '20-100-1-15-1572" & _
                        "', line_amounts0" & Trim(Str(pubCounter)) & " = " & pubShipmentAmt2 & _
                        ", line_vatcodes0" & Trim(Str(pubCounter)) & " = '   ', " & _
                        "line_vat_amts0" & Trim(Str(pubCounter)) & " = 0, " & _
                        "line_narrative0" & Trim(Str(pubCounter)) & " = 'PRICE VARIANCE" & _
                        "', rept_curr_cd0" & Trim(Str(pubCounter)) & " = 'SAR', " & _
                        "curr_code0" & Trim(Str(pubCounter)) & " = 'SAR'," & _
                        "curr_type0" & Trim(Str(pubCounter)) & " = 'YY'," & _
                        "fix_rate_ind0" & Trim(Str(pubCounter)) & "= 'N'," & _
                        "curr_freq0" & Trim(Str(pubCounter)) & "= 'Y'," & _
                        "multidiv0" & Trim(Str(pubCounter)) & "= '*'," & _
                        "revalue_status0" & Trim(Str(pubCounter)) & "='O'," & _
                        "trans_amount0" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt2 & _
                        ",report_amount0" & Trim(Str(pubCounter)) & "= " & pubShipmentAmt2 & _
                        ",exchange_rate0" & Trim(Str(pubCounter)) & "= 1," & _
                        "line_key0" & Trim(Str(pubCounter)) & "= " & pubCounter & _
                        " where journal_no = '" & pubKeyValue & "' and page_no = '" & pubNljrnmPageNum & "'"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub GetServerDateTime()

        Dim ConnStr As String
        Dim sSql As String
        Dim mServerdate As Date
        ConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "select top 1 getdate() as logdate from and_poshipment_master"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then

                While mReader.Read()

                    mServerdate = mReader("logdate".ToString)

                    pubServerDate = Format(mServerdate, "dd MMM yyyy")

                End While

            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub GetEndBal()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=seslsvrho;User ID=scheme;Password=Er1c550n2;Initial Catalog=SES"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "SELECT mat_cost+variance+standard as endbal, customs+freight+insurance+variance2+handling as endbal2, " & _
                        "(mat_cost+variance+standard)+(customs+freight+insurance+variance2+handling) as endbal3 FROM " & _
                    "and_poshipment where shipment_no = '" & pubShipmentNo.Trim & "'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()


                    pubEndBalance = mReader("endbal")
                    pubEndBalance2 = mReader("endbal2")
                    pubEndBalance3 = mReader("endbal3")

                    pubTransAmt = Format(pubEndBalance, "##0.00")
                    pubTransAmt2 = Format(pubEndBalance2, "##0.00")
                    pubTransAmt3 = Format(pubEndBalance3, "##0.00")

                    pubShipmentAmt = pubEndBalance * -1
                    pubShipmentAmt2 = pubEndBalance2 * -1
                    pubShipmentAmt3 = pubEndBalance3 * -1

                    'pubTotalCredit = pubTotalCredit + pubEndBalance
                    'pubTotalCredit2 = pubTotalCredit2 + pubEndBalance2
                    pubTotalCredit3 = pubTotalCredit3 + pubEndBalance3

                End While

            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub AddJournal1570()

        '1570
        If pubTransAmt <> 0 Then

            pubCounter = pubCounter + 1
            pubCounterRec = pubCounterRec + 1

            If pubCounterRec <= 10 Then
                pubPrjxmPageNum = "   "
                pubNljrnmPageNum = "    "
            Else
                If pubCounterRec Mod 10 <> 0 Then
                    If pubCounterRec > 100 Then
                        If pubCounterRec > 1000 Then
                            pubPrjxmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                            pubNljrnmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                        Else
                            pubPrjxmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                            pubNljrnmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                        End If

                    Else
                        pubPrjxmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                        pubNljrnmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                    End If
                End If

            End If

            Select Case pubCounter
                Case 1
                    InsertRecordNljrnm1570()
                    UpdateFieldsWithBlankNljrnm()

                    InsertRecordNlanalm()
                    InsertRecordPrjxm()
                    UpdateFieldsWithBlankPrjxm()

                Case 2 To 10

                    'insert shipment account - 1570
                    UpdateShipmentNljrnm()
                    InsertShipmentNlanalm()
                    UpdateShipmentPrjxm()

            End Select

            If pubCounter = 10 Then
                UpdateNljrnmNoLines()
                pubCounter = 0
            Else
                UpdateNljrnmNoLines()
            End If

        End If

    End Sub

    Private Sub AddJournal1572()

        '1572
        If pubTransAmt2 <> 0 Then


            pubCounter = pubCounter + 1
            pubCounterRec = pubCounterRec + 1

            If pubCounterRec <= 10 Then
                pubPrjxmPageNum = "   "
                pubNljrnmPageNum = "    "
            Else
                If pubCounterRec Mod 10 <> 0 Then

                    If pubCounterRec > 100 Then
                        If pubCounterRec > 1000 Then
                            pubPrjxmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                            pubNljrnmPageNum = Mid(Trim(Str(pubCounterRec)), 1, 3) & " "
                        Else
                            pubPrjxmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                            pubNljrnmPageNum = "0" & Mid(Trim(Str(pubCounterRec)), 1, 2) & " "
                        End If

                    Else
                        pubPrjxmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                        pubNljrnmPageNum = "00" & Mid(Trim(Str(pubCounterRec)), 1, 1) & " "
                    End If

                End If

            End If

            Select Case pubCounter
                Case 1

                    InsertRecordNljrnm1572()
                    UpdateFieldsWithBlankNljrnm()

                    InsertRecordNlanalm()
                    InsertRecordPrjxm()
                    UpdateFieldsWithBlankPrjxm()

                Case 2 To 10

                    'insert shipment account - 1572
                    UpdateShipmentNljrnm2()
                    InsertShipmentNlanalm()
                    UpdateShipmentPrjxm()

            End Select

            If pubCounter = 10 Then
                UpdateNljrnmNoLines()
                pubCounter = 0
            Else
                UpdateNljrnmNoLines()
            End If

        End If

    End Sub

    Private Sub AddJournal()

        If pubTransAmt3 <> 0 Then


            If UCase(Mid(pubSalesOrder.Trim, 1, 3)) = "VAR" Or UCase(Mid(pubSalesOrder.Trim, 1, 2)) = "CC" Or _
                UCase(Mid(pubSalesOrder.Trim, 1, 5)) = "STOCK" Or Val(Mid(pubSalesOrder.Trim, 1, 1)) = 0 Then
                pubPostingCode = "20-100-1-42-4024"
                pubSalesOrder = Space(20)
                pubProductGroup = Space(10)
                pubExpenseCode = Space(20)

            Else
                CheckProjectStatus()

                If pubProjectType.Trim = "ASIN" Then
                    pubSalesOrder = pubSalesOrder.Trim & "/S"
                End If

                If pubProjectStatus = "OPEN" Then
                    pubPostingCode = pubProjectCc.Trim & "-13-1320"
                    pubProductGroup = "Y98099099"
                    pubExpenseCode = "MAT-OTHERS"
                Else
                    CheckPostStatus()
                    If pubPostStatus = "O" Then
                        pubPostingCode = "20-100-1-42-4022"
                        pubExpenseCode = "MAT-OTHERS"
                        pubProductGroup = "Y98099099"
                    Else
                        pubPostingCode = pubProjectCc.Trim & "-42-4024"

                        CheckProjectCCVariance()

                        pubSalesOrder = Space(20)
                        pubProductGroup = Space(10)
                        pubExpenseCode = Space(20)
                    End If
                End If
            End If

            'Accounts
            Select Case pubCounter
                Case 1
                    InsertRecordNljrnm()
                    UpdateFieldsWithBlankNljrnm()

                    InsertRecordNlanalm()
                    InsertRecordPrjxm()
                    UpdateFieldsWithBlankPrjxm()

                Case 2 To 10

                    UpdateNljrnm()
                    InsertRecordNlanalm()
                    UpdatePrpjxm()

            End Select

            If pubCounter = 10 Then
                UpdateNljrnmNoLines()
                pubCounter = 0
            End If

        End If

    End Sub

    Private Sub CheckProjectCCVariance()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select nominal_code from nlmastm where nominal_code =  '" & pubPostingCode.Trim & "'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If Not mReader.HasRows Then
                pubPostingCode = pubProjectCc.Trim & "-41-4021"
            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub UpdateFieldsWithBlankNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "update sir_nljrnm set nominal_codes02 = '" & Space(16) & "', nominal_codes03 = '" & Space(16) & "'," & _
        "nominal_codes04 = '" & Space(16) & "', nominal_codes05 = '" & Space(16) & "', nominal_codes06 = '" & Space(16) & "'," & _
        "nominal_codes07 = '" & Space(16) & "', nominal_codes08 = '" & Space(16) & "', nominal_codes09 = '" & Space(16) & "', nominal_codes10 = '" & Space(16) & "'," & _
        "line_amounts02 = 0, line_amounts03 = 0, line_amounts04 = 0, line_amounts05 = 0, line_amounts06 = 0, line_amounts07 = 0, line_amounts08 = 0, line_amounts09 = 0, line_amounts10 = 0, " & _
        "line_vatcodes02 = '   ', line_vatcodes03= '   ', line_vatcodes04= '   ', line_vatcodes05= '   ', line_vatcodes06= '   ', line_vatcodes07= '   ', " & _
        "line_vatcodes08= '   ', line_vatcodes09= '   ', line_vatcodes10= '   '," & _
        "line_vat_amts02 = 0, line_vat_amts03 = 0, line_vat_amts04 = 0, line_vat_amts05 = 0, line_vat_amts06 = 0, line_vat_amts07 = 0, line_vat_amts08 = 0, " & _
        "line_vat_amts09 = 0, line_vat_amts10 = 0, line_narrative02 = '" & Space(20) & "', line_narrative03 = '" & Space(20) & "', line_narrative04 = '" & Space(20) & "', " & _
        "line_narrative05 = '" & Space(20) & "', line_narrative06 = '" & Space(20) & "', line_narrative07 = '" & Space(20) & "', line_narrative08 = '" & Space(20) & "', " & _
        "line_narrative09 = '" & Space(20) & "', line_narrative10 = '" & Space(20) & "', rept_curr_cd02 = '   ', rept_curr_cd03 = '   ', rept_curr_cd04 = '   ', " & _
        "rept_curr_cd05 = '   ', rept_curr_cd06 = '   ', rept_curr_cd07 = '   ', rept_curr_cd08 = '   ', rept_curr_cd09 = '   ', rept_curr_cd10 = '   ', " & _
        "curr_code02 = '   ', curr_code03 = '   ', curr_code04 = '   ', curr_code05 = '   ', curr_code06 = '   ', curr_code07 = '   ', curr_code08 = '   ', " & _
        "curr_code09 = '   ', curr_code10 = '   ', curr_type02 = '  ', curr_type03 = '  ', curr_type04 = '  ', curr_type05 = '  ', curr_type06 = '  ', " & _
        "curr_type07 = '  ', curr_type08 = '  ', curr_type09 = '  ', curr_type10 = '  ', fix_rate_ind02 = ' ', fix_rate_ind03 = ' ', fix_rate_ind04 = ' ', " & _
        "fix_rate_ind05 = ' ', fix_rate_ind06 = ' ', fix_rate_ind07 = ' ', fix_rate_ind08 = ' ', fix_rate_ind09 = ' ', fix_rate_ind10 = ' ', " & _
        "curr_freq02 = ' ', curr_freq03 = ' ', curr_freq04 = ' ', curr_freq05 = ' ', curr_freq06 = ' ', curr_freq07 = ' ', curr_freq08 = ' ', curr_freq09 = ' ', curr_freq10 = ' ', " & _
        "multidiv02 = ' ', multidiv03 = ' ', multidiv04 = ' ', multidiv05 = ' ', multidiv06 = ' ', multidiv07 = ' ', multidiv08 = ' ', multidiv09 = ' ', multidiv10 = ' ', " & _
        "revalue_status02 = ' ', revalue_status03 = ' ', revalue_status04 = ' ', revalue_status05 = ' ', revalue_status06 = ' ', revalue_status07 = ' ', revalue_status08 = ' ', " & _
        "revalue_status09 = ' ', revalue_status10 = ' ', trans_amount02 = 0, trans_amount03 = 0, trans_amount04 = 0, trans_amount05 = 0, trans_amount06 = 0, trans_amount07 = 0, " & _
        "trans_amount08 = 0, trans_amount09 = 0, trans_amount10 = 0, report_amount02 = 0, report_amount03 = 0, report_amount04 = 0, report_amount05 = 0, report_amount06 = 0, " & _
        "report_amount07 = 0, report_amount08 = 0, report_amount09 = 0, report_amount10 = 0, exchange_rate02 = '1', exchange_rate03 = '1', exchange_rate04 = '1', " & _
        "exchange_rate05 = '1', exchange_rate06 = '1', exchange_rate07 = '1', exchange_rate08 = '1', exchange_rate09 = '1', exchange_rate10 = '1', " & _
        "line_key02 = 0, line_key03 = 0, line_key04 = 0, line_key05 = 0, line_key06 = 0, line_key07 = 0, line_key08 = 0, line_key09 = 0, line_key10 = 0 " & _
        " where journal_no = '" & pubKeyValue.Trim & "' and page_no = '" & pubNljrnmPageNum & "'"
        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateFieldsWithBlankPrjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "update sir_prjxm set jobcode02 = '" & Space(20) & "', jobcode03 = '" & Space(20) & "', jobcode04 = '" & Space(20) & "', " & _
                "jobcode05 = '" & Space(20) & "', jobcode06 = '" & Space(20) & "', jobcode07 = '" & Space(20) & "', jobcode08 = '" & Space(20) & "', " & _
                "jobcode09 = '" & Space(20) & "', jobcode10 = '" & Space(20) & "', jobcode11 = '" & Space(20) & "', jobcode12 = '" & Space(20) & "', " & _
                "jobcode13 = '" & Space(20) & "', jobcode14 = '" & Space(20) & "', jobcode15 = '" & Space(20) & "', jobcode16 = '" & Space(20) & "', " & _
                "jobcode17 = '" & Space(20) & "', jobcode18 = '" & Space(20) & "', jobcode19 = '" & Space(20) & "', jobcode20 = '" & Space(20) & "', " & _
                "jobcode21 = '" & Space(20) & "', jobcode22 = '" & Space(20) & "', jobcode23 = '" & Space(20) & "', jobcode24 = '" & Space(20) & "', " & _
                "jobcode25 = '" & Space(20) & "', jobcode26 = '" & Space(20) & "', jobcode27 = '" & Space(20) & "', jobcode28 = '" & Space(20) & "', " & _
                "jobcode29 = '" & Space(20) & "', jobcode30 = '" & Space(20) & "', jobcode31 = '" & Space(20) & "', expcode02 = '" & Space(20) & "', " & _
                "expcode03 = '" & Space(20) & "', expcode04 = '" & Space(20) & "', expcode05 = '" & Space(20) & "', expcode06 = '" & Space(20) & "', " & _
                "expcode07 = '" & Space(20) & "', expcode08 = '" & Space(20) & "', expcode09 = '" & Space(20) & "', expcode10 = '" & Space(20) & "', " & _
                "expcode11 = '" & Space(20) & "', expcode12 = '" & Space(20) & "', expcode13 = '" & Space(20) & "', expcode14 = '" & Space(20) & "', " & _
                "expcode15 = '" & Space(20) & "', expcode16 = '" & Space(20) & "', expcode17 = '" & Space(20) & "', expcode18 = '" & Space(20) & "', " & _
                "expcode19 = '" & Space(20) & "', expcode20 = '" & Space(20) & "', expcode21 = '" & Space(20) & "', expcode22 = '" & Space(20) & "', " & _
                "expcode23 = '" & Space(20) & "', expcode24 = '" & Space(20) & "', expcode25 = '" & Space(20) & "', expcode26 = '" & Space(20) & "', " & _
                "expcode27 = '" & Space(20) & "', expcode28 = '" & Space(20) & "', expcode29 = '" & Space(20) & "', expcode30 = '" & Space(20) & "', " & _
                "expcode31 = '" & Space(20) & "' where jrn_num = '" & pubKeyValue.Trim & "' and page_num = '" & pubPrjxmPageNum & "'"
        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()


    End Sub

    Private Sub InsertIntoLiveNljrnm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "insert into nljrnm(journal_no, slash, page_no, dated, narrative, hold_indicator, action_indicator, period_indicator, period_entered, " & _
                    "expiry_date, total_debits, total_credits, no_of_lines, total_no_of_lines, nominal_codes01, nominal_codes02, nominal_codes03, " & _
                    "nominal_codes04, nominal_codes05, nominal_codes06,nominal_codes07, nominal_codes08, nominal_codes09, nominal_codes10, line_amounts01, " & _
                    "line_amounts02, line_amounts03, line_amounts04,line_amounts05, line_amounts06, line_amounts07, line_amounts08, line_amounts09, " & _
                    "line_amounts10, line_vatcodes01, line_vatcodes02,line_vatcodes03, line_vatcodes04, line_vatcodes05, line_vatcodes06, line_vatcodes07, " & _
                    "line_vatcodes08, line_vatcodes09, line_vatcodes10,line_vat_amts01, line_vat_amts02, line_vat_amts03, line_vat_amts04, line_vat_amts05, " & _
                    "line_vat_amts06, line_vat_amts07, line_vat_amts08,line_vat_amts09, line_vat_amts10, line_narrative01, line_narrative02, line_narrative03, " & _
                    "line_narrative04, line_narrative05, line_narrative06,line_narrative07, line_narrative08, line_narrative09, line_narrative10, nlyear, " & _
                    "transdate, source1, source2, rept_curr_cd01, rept_curr_cd02,rept_curr_cd03, rept_curr_cd04, rept_curr_cd05, rept_curr_cd06, rept_curr_cd07, " & _
                    "rept_curr_cd08, rept_curr_cd09, rept_curr_cd10, curr_code01,curr_code02, curr_code03, curr_code04, curr_code05, curr_code06, curr_code07, " & _
                    "curr_code08, curr_code09, curr_code10, curr_type01, curr_type02,curr_type03, curr_type04, curr_type05, curr_type06, curr_type07, " & _
                    "curr_type08, curr_type09, curr_type10, fix_rate_ind01, fix_rate_ind02, fix_rate_ind03,fix_rate_ind04, fix_rate_ind05, fix_rate_ind06, " & _
                    "fix_rate_ind07, fix_rate_ind08, fix_rate_ind09, fix_rate_ind10, curr_freq01, curr_freq02, curr_freq03,curr_freq04, curr_freq05, " & _
                    "curr_freq06, curr_freq07, curr_freq08, curr_freq09, curr_freq10, multidiv01, multidiv02, multidiv03, multidiv04, multidiv05," & _
                    "multidiv06, multidiv07, multidiv08, multidiv09, multidiv10, revalue_status01, revalue_status02, revalue_status03, revalue_status04, " & _
                    "revalue_status05,revalue_status06, revalue_status07, revalue_status08, revalue_status09, revalue_status10, analysis1, analysis2, " & _
                    "analysis3, trans_amount01,trans_amount02, trans_amount03, trans_amount04, trans_amount05, trans_amount06, trans_amount07, " & _
                    "trans_amount08, trans_amount09,trans_amount10, report_amount01, report_amount02, report_amount03, report_amount04, " & _
                    "report_amount05, report_amount06, report_amount07,report_amount08, report_amount09, report_amount10, exchange_rate01, " & _
                    "exchange_rate02, exchange_rate03, exchange_rate04, exchange_rate05,exchange_rate06, exchange_rate07, exchange_rate08, " & _
                    "exchange_rate09, exchange_rate10, interco_flag, transaction_group, group_journal,journal_type, last_line_key, line_key01, " & _
                    "line_key02, line_key03, line_key04, line_key05, line_key06, line_key07, line_key08, line_key09, line_key10) " & _
                    "Select * from sir_nljrnm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertIntoLiveNlanalm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "insert into nlanalm(journal, page_no, line, analysis1, analysis2, analysis3) " & _
                "Select * from sir_nlanalm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub InsertIntoLivePrjxm()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        cmdUpdate.CommandText = "insert into prjxm(jrn_num, delimiter, page_num, jobcode01, jobcode02, jobcode03, jobcode04, jobcode05, jobcode06, " & _
                                "jobcode07, jobcode08, jobcode09, jobcode10, jobcode11, jobcode12, jobcode13, jobcode14, jobcode15, jobcode16, " & _
                                "jobcode17, jobcode18, jobcode19, jobcode20, jobcode21, jobcode22, jobcode23, jobcode24, jobcode25, jobcode26, " & _
                                "jobcode27, jobcode28, jobcode29, jobcode30, jobcode31, expcode01, expcode02, expcode03, expcode04, expcode05, " & _
                                "expcode06, expcode07, expcode08, expcode09, expcode10, expcode11, expcode12, expcode13, expcode14, expcode15, " & _
                                "expcode16, expcode17, expcode18, expcode19, expcode20, expcode21, expcode22, expcode23, expcode24, expcode25, " & _
                                "expcode26, expcode27, expcode28, expcode29, expcode30, expcode31) Select * from sir_prjxm"

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub UpdateNljrnmNoLines()

        Dim strConnStr As String
        strConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        If pubCounter = 10 Then
            cmdUpdate.CommandText = "update sir_nljrnm set no_of_lines = '10 ' where journal_no = '" & _
                                    pubKeyValue.Trim & "' and page_no = '" & pubNljrnmPageNum & "'"

        Else
            cmdUpdate.CommandText = "update sir_nljrnm set no_of_lines = '" & Trim(Str(pubCounter)) & "  ' where journal_no = '" & _
                                    pubKeyValue.Trim & "' and page_no = '" & pubNljrnmPageNum & "'"

        End If

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Private Sub CheckProjectStatus()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select project_type, project_status, cost_centre from projects where project_code = '" & pubSalesOrder.Trim & "'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    pubProjectStatus = Trim(mReader("project_status"))
                    pubProjectType = Trim(mReader("project_type"))
                    pubProjectCc = Trim(mReader("cost_centre"))

                End While
            Else
                pubProjectStatus = "CLOSE"
                pubProjectType = " "

            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub CheckPostStatus()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SAGE;User ID=scheme;Password=Password1;Initial Catalog=cs3live"
        Dim MySqlConn As New SqlConnection(ConnStr)
        MySqlConn.Open()
        Try

            sSql = "Select post_status from prpjm where project_code = '" & pubSalesOrder.Trim & "'"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()
                    pubPostStatus = mReader("post_status")
                End While
            Else
                pubPostStatus = "C"
            End If

        Catch ex As Exception

        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub UpdateTagWithError()

        Dim ConnStr As String
        Dim sSql As String

        ConnStr = "Data Source=SESLSVRHO;User ID=scheme;Password=Er1c550n2"

        Dim MySqlConn As New SqlConnection(ConnStr)

        MySqlConn.Open()
        Try

            sSql = "Select analysis_code1, category, sum(journal_amount) as sumamount from SES.scheme.and_poshipment_master where analysis_code1 in " & _
                    "(Select a.shipment_no from and_poshipment a inner join SAGE.cs3live.scheme.poheadm b on " & _
                    "a.shipment_no = b.order_no where a.tag =  'E') group by analysis_code1, category"

            Dim MySqlCmd As New SqlCommand(sSql, MySqlConn)
            Dim mReader As SqlDataReader

            mReader = MySqlCmd.ExecuteReader()
            If mReader.HasRows Then
                While mReader.Read()

                    pubAnalysisCode1 = Trim(mReader("analysis_code1"))
                    pubCategory = Trim(mReader("category"))
                    pubCatAmount = mReader("sumamount")

                    UpdateAndPoShipmentCat()

                End While
            End If

        Catch ex As Exception
            MySqlConn.Close()
        Finally
            MySqlConn.Close()
        End Try

    End Sub

    Private Sub UpdateAndPoShipmentCat()

        Dim strConnStr As String
        strConnStr = "Data Source=SESLSVRHO;User ID=scheme;Password=Er1c550n2"
        Dim MySqlConn As New SqlConnection(strConnStr)

        Dim cmdUpdate As New SqlCommand

        Select Case pubCategory.Trim
            Case "M"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set mat_cost = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
            Case "C"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set customs = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
            Case "F"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set freight = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
            Case "I"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set insurance = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
            Case "S"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set standard = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
            Case "V"
                cmdUpdate.CommandText = "update SES.scheme.and_poshipment set variance = " & pubCatAmount & _
                                            ", tag = 'C' where shipment_no = '" & pubAnalysisCode1.Trim & "'"
        End Select

        cmdUpdate.Connection = MySqlConn
        cmdUpdate.Connection.Open()
        cmdUpdate.ExecuteNonQuery()
        MySqlConn.Close()

    End Sub

    Protected Sub Button8_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Button8.Text.Trim = "History" Then
            MultiView1.ActiveViewIndex = 4
            Button8.Text = "Live"
            Button7.Visible = False
            Button1.Visible = False
            Button2.Visible = False
            Button4.Visible = False
            Button6.Visible = False
            Button9.Visible = False
        Else
            MultiView1.ActiveViewIndex = 1
            Button8.Text = "History"
            Button7.Visible = True
            Button1.Visible = True
            Button2.Visible = True
            Button4.Visible = True
            Button6.Visible = True
            Button9.Visible = True
        End If

    End Sub

    Protected Sub GridView10_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView10.RowCommand

        Select Case e.CommandName

            Case "Select"

                Dim rowindex As Integer = CInt(e.CommandArgument)

                Label1.Text = rowindex.ToString.Trim

                Button1.Visible = False
                Button2.Visible = False
                Button4.Visible = False
                Button6.Visible = False
                Button7.Visible = False
                Button8.Visible = False
                Button9.Visible = False

                Label9.Visible = False
                Label10.Visible = False
                Label11.Visible = False
                Label12.Visible = False
                Label13.Visible = False
                Label14.Visible = False

                DisplayLabels()

                MultiView1.ActiveViewIndex = 2

        End Select

    End Sub

    Protected Sub GridView10_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView10.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            ' add the Total to the running total variables
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "mat_cost")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "mat_cost")) = 0 Then
                    e.Row.Cells(1).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "customs")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "customs")) = 0 Then
                    e.Row.Cells(2).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "freight")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "freight")) = 0 Then
                    e.Row.Cells(3).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "insurance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "insurance")) = 0 Then
                    e.Row.Cells(4).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "variance")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "variance")) = 0 Then
                    e.Row.Cells(5).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "actual")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "actual")) = 0 Then
                    e.Row.Cells(6).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "standard")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "standard")) = 0 Then
                    e.Row.Cells(7).Text = ""
                End If
            End If

            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "endbal")) Then
                If Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "endbal")) = 0 Then
                    e.Row.Cells(8).Text = ""
                End If
            End If

        End If

    End Sub

    Protected Sub Button9_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckedAllItems()
    End Sub

    Private Sub CheckedAllItems()

        Dim dr As GridViewRow
        Dim gIndex As Integer = -1
        For Each dr In GridView2.Rows

            If gIndex = -1 Then
                gIndex = 0
            End If

            Dim RowCheckBox As CheckBox = CType(GridView2.Rows(gIndex).FindControl("CheckBox1"), CheckBox)

            RowCheckBox.Checked = True

            gIndex += 1

        Next

    End Sub

    Protected Sub GridView2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView2.Sorting
        SummPOShipment()
    End Sub

End Class
