<%@ Page Language="VB" masterpagefile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Shipment.aspx.vb" Inherits="Shipment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <atlas:ScriptManager ID="sl" EnablePartialRendering="true" runat="server"/>
    <atlas:UpdatePanel ID="pl1" runat="server">
    <ContentTemplate>
    
    <div class="header">

       <atlas:UpdateProgress ID="progress1" runat="server">
            <ProgressTemplate>
                <div class="progress" style="font-weight: bold; font-size: 8pt; left: 460px; color: #0033cc; font-family: Verdana; position: absolute; top: 20px;">
                    In Progress......<img src="images/in_progress.gif" />
                </div>
            
            </ProgressTemplate>
        
        </atlas:UpdateProgress>        
        
    <table border="0" cellpadding="0" cellspacing="0" style="background-color: honeydew; border-top: lightslategray 2px solid; height: 33px; width: 100%;">
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label15" runat="server" ForeColor="Honeydew" Visible="False" Width="45px"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label8" runat="server" ForeColor="Honeydew" Visible="False"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Width="54px" ForeColor="Honeydew" Visible="False"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button9" runat="server" Text="Check All" OnClick="Button9_Click" Width="70px" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button8" runat="server" Text="History" Width="81px" OnClick="Button8_Click" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button7" runat="server" Text="Refresh" Width="75px" OnClick="Button7_Click" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button1" runat="server" Text="UnCheck All" OnClick="Button1_Click" Width="99px" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button2" runat="server" Text="Post Variance" Width="116px" OnClick="Button2_Click" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button4" runat="server" Text="Disable Paging" OnClick="Button4_Click" Width="111px" />
            </td>
            <td style="width: 100px; text-align: right;">
                <asp:Button ID="Button6" runat="server" Text="Preview/Print" OnClick="Button6_Click" Width="96px" />
                </td>
            <td style="width: 100px; text-align: right;">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://inside.saudi-ericsson.com/sesreports/tabporeport.aspx" Target="_blank" ToolTip="Open Outstanding Purchase Orders" Width="178px">Outstanding Purchase Orders</asp:HyperLink>
                </td>
        </tr>
    </table>

        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="shipment_no" DataSourceID="ObjectDataSource1" ForeColor="#333333"
                    GridLines="None" Width="100%" AllowPaging="True" PageSize="20">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Shipment Order No." SortExpression="shipment_no">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ForeColor="royalBlue" Text='<%# Bind("shipment_no") %>' 
                            CommandArgument='<%# Eval("shipment_no") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material Cost" SortExpression="mat_cost">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("mat_cost","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToString(Eval("variance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Standard Material" SortExpression="standard">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("standard","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Balance Material" SortExpression="end_bal_material">
                            <ItemTemplate>
                                <asp:Label ID="Label10" runat="server" Text='<%# Convert.ToString(Eval("end_bal_material","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Customs Duty" SortExpression="customs">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Convert.ToString(Eval("customs","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Freight Cost" SortExpression="freight">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Convert.ToString(Eval("freight","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Insurance Cost" SortExpression="insurance">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToString(Eval("insurance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance2">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToString(Eval("variance2","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Handling" SortExpression="handling">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Convert.ToString(Eval("handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Balance Handling" SortExpression="end_bal_handling">
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Convert.ToString(Eval("end_bal_handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>                                                
                        <asp:TemplateField HeaderText="Shipment End Balance" SortExpression="endbal">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Convert.ToString(Eval("endbal","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" ReadOnly="True" />
                        <asp:BoundField DataField="salesorder" HeaderText="Sales Order" SortExpression="salesorder" ReadOnly="True" />
                    </Columns>
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Height="30px" />
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#FFCC66" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataPOShipmentList"
                    TypeName="POShipmentListTableAdapters.and_poshipmentTableAdapter">
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="shipment_no" DataSourceID="ObjectDataSource2" ForeColor="#333333"
                    GridLines="None" Width="100%" AllowPaging="True" PageSize="20">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="False" />
                            </ItemTemplate>                        
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shipment Order No." SortExpression="shipment_no">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ForeColor="royalBlue" Text='<%# Bind("shipment_no") %>' 
                            CommandArgument='<%# Eval("shipment_no") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material Cost" SortExpression="mat_cost">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("mat_cost","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="Gold" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToString(Eval("variance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="Gold" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Standard Material" SortExpression="standard">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("standard","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="Gold" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="End Balance Material" SortExpression="end_bal_material">
                            <ItemTemplate>
                                <asp:Label ID="Label10" runat="server" Text='<%# Convert.ToString(Eval("end_bal_material","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="Gold" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Customs Duty" SortExpression="customs">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Convert.ToString(Eval("customs","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Freight Cost" SortExpression="freight">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Convert.ToString(Eval("freight","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Insurance Cost" SortExpression="insurance">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToString(Eval("insurance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance2">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToString(Eval("variance2","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Handling" SortExpression="handling">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Convert.ToString(Eval("handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="End Balance Handling" SortExpression="end_bal_handling">
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Convert.ToString(Eval("end_bal_handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="CornflowerBlue" />
                        </asp:TemplateField>                                                                        
                        <asp:TemplateField HeaderText="Shipment End Balance" SortExpression="endbal">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Convert.ToString(Eval("endbal","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle ForeColor="PaleGreen" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" ReadOnly="True" />
                        <asp:BoundField DataField="salesorder" HeaderText="Sales Order" SortExpression="salesorder" ReadOnly="True" />
                    </Columns>
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Height="30px" />
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#FFCC66" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataPOShipmentList"
                    TypeName="POShipmentListTableAdapters.and_poshipmentTableAdapter">
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="shipment_no" DataSourceID="ObjectDataSource3" ForeColor="#333333"
                    GridLines="None" Width="100%">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" Height="20px" />
                    <Columns>
                        <asp:BoundField DataField="shipment_no" HeaderText="Shipment" ReadOnly="True" SortExpression="shipment_no" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Material Cost" SortExpression="mat_cost">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("mat_cost","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customs Duty" SortExpression="customs">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Convert.ToString(Eval("customs","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Freight Cost" SortExpression="freight">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Convert.ToString(Eval("freight","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Insurance Cost" SortExpression="insurance">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToString(Eval("insurance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToString(Eval("variance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Actual Cost" SortExpression="actual">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToString(Eval("actual","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Material" SortExpression="standard">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Convert.ToString(Eval("standard","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Handling" SortExpression="handling">
                            <ItemTemplate>
                                <asp:Label ID="Label10" runat="server" Text='<%# Convert.ToString(Eval("handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="End Balance" SortExpression="endbal">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Convert.ToString(Eval("endbal","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                        <asp:BoundField DataField="salesorder" HeaderText="Sales Order" SortExpression="salesorder" />

                    </Columns>
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataPOSpecific" TypeName="POSpecificTableAdapters.and_poshipmentTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td style="width: 100px">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Text="Material Cost" Width="135px" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
                            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataSourceID="ObjectDataSource4" ForeColor="#333333" GridLines="Vertical" Width="100%" ShowFooter="True">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                                    <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                                    <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                                    <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                                    <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                                    <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                                    <asp:Parameter DefaultValue="M" Name="pocategory" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                  <br />
                <table>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label10" runat="server" Text="Customs Duty" Font-Bold="True" Width="127px" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="ObjectDataSource5" ForeColor="Black" GridLines="Vertical" ShowFooter="True" Width="100%">
                    <FooterStyle BackColor="#CCCC99" />
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                        <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                        <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                        <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                        <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                        <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                        <asp:Parameter DefaultValue="C" Name="pocategory" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table style="width: 166px">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Text="Freight Cost" Width="111px" Visible="False"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="ObjectDataSource6" ForeColor="#333333" GridLines="Vertical" ShowFooter="True" Width="100%">
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                        <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                        <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                        <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                        <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                        <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource6" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                        <asp:Parameter DefaultValue="F" Name="pocategory" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table style="width: 182px">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Insurance Cost" Width="110px" Visible="False"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="ObjectDataSource7" ForeColor="#333333" GridLines="Vertical" ShowFooter="True" Width="100%">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                        <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                        <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                        <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                        <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                        <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource7" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" DefaultValue="" Name="ponum" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter DefaultValue="I" Name="pocategory" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table style="width: 216px">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label14" runat="server" Font-Bold="True" Text="Standard Cost" Width="179px" Visible="False"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="GridView9" runat="server" AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" DataSourceID="ObjectDataSource9" ForeColor="Black" GridLines="Vertical" ShowFooter="True" Width="100%">
                    <FooterStyle BackColor="Tan" />
                    <Columns>
                        <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                        <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                        <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                        <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                        <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                        <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource9" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                        <asp:Parameter DefaultValue="S" Name="pocategory" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table style="width: 187px">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="Variance" Width="154px" Visible="False"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="GridView8" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="ObjectDataSource8" ShowFooter="True" Width="100%">
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <Columns>
                        <asp:BoundField DataField="journal_number" HeaderText="Journal" SortExpression="journal_number" />
                        <asp:BoundField DataField="journal_desc" HeaderText="Description" SortExpression="journal_desc" />
                        <asp:BoundField DataField="period" HeaderText="Period" ReadOnly="True" SortExpression="period" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="journal_date" HeaderText="Date" SortExpression="journal_date" DataFormatString="{0:dd-MM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="analysis_code1" HeaderText="Trans ID 1" SortExpression="analysis_code1" />
                        <asp:BoundField DataField="analysis_code2" HeaderText="Trans ID 2" SortExpression="analysis_code2" />
                        <asp:BoundField DataField="analysis_code3" HeaderText="Trans ID 3" SortExpression="analysis_code3" />
                        <asp:TemplateField HeaderText="Debit" SortExpression="gldebit">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("gldebit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="glcredit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("glcredit","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource8" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataJournals" TypeName="JournalsTableAdapters.and_poshipment_masterTableAdapter">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label1" Name="ponum" PropertyName="Text" Type="String" />
                        <asp:Parameter DefaultValue="V" Name="pocategory" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />                
                <asp:Button ID="Button3" runat="server" Text="Shipment List" OnClick="Button3_Click" Width="126px" />
                
            </asp:View>
            <asp:View ID="View4" runat="server">
                <asp:Button ID="Button5" runat="server" Text="Shipment List" Width="123px" OnClick="Button5_Click" /><br />
                
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" ProcessingMode="Remote" ShowDocumentMapButton="False" ShowFindControls="False" Width="100%">
                    <ServerReport ReportPath="/Archive/Shipment" ReportServerUrl="http://inside.saudi-ericsson.com/reportserver" />
                </rsweb:ReportViewer>
            </asp:View>
            <asp:View ID="View5" runat="server">
                <asp:GridView ID="GridView10" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="shipment_no" DataSourceID="ObjectDataSource10" ForeColor="#333333"
                    GridLines="None" Width="100%" AllowPaging="True" PageSize="20">
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Shipment Order No." SortExpression="shipment_no">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ForeColor="royalBlue" Text='<%# Bind("shipment_no") %>' 
                            CommandArgument='<%# Eval("shipment_no") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Material Cost" SortExpression="mat_cost">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Convert.ToString(Eval("mat_cost","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customs Duty" SortExpression="customs">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Convert.ToString(Eval("customs","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Freight Cost" SortExpression="freight">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Convert.ToString(Eval("freight","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Insurance Cost" SortExpression="insurance">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToString(Eval("insurance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variance" SortExpression="variance">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToString(Eval("variance","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Actual Cost" SortExpression="actual">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToString(Eval("actual","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Material" SortExpression="standard">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToString(Eval("standard","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Standard Handling" SortExpression="handling">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Convert.ToString(Eval("handling","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Shipment End Balance" SortExpression="endbal">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Convert.ToString(Eval("endbal","{0:c}")).Replace("$", " ") %>' ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" ReadOnly="True" />
                        <asp:BoundField DataField="salesorder" HeaderText="Sales Order" SortExpression="salesorder" ReadOnly="True" />
                    </Columns>
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Height="30px" />
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#FFCC66" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource10" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataShipmentHistory" TypeName="POShipmentHistoryTableAdapters.and_poshipmentTableAdapter">
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>


        
        
</div>            
    
</ContentTemplate>   
</atlas:UpdatePanel>    
   
</asp:Content>    