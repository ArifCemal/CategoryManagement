<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Online_Store_Management.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="Screen" runat="server" Height="35px">
        <asp:DropDownList ID="Ana_Kategoriler" runat="server"  AutoPostBack="true"
       
            onselectedindexchanged="Ana_Kategoriler_SelectedIndexChanged"  >
       <items> 
       <asp:ListItem Text="Kategoriler:"  Selected="false" ></asp:ListItem>
       </items>
    
        </asp:DropDownList>
           
 
     
     
        <asp:DropDownList ID="Alt_Kategoriler" runat="server"  AutoPostBack="true"
            onselectedindexchanged="Alt_Kategoriler_SelectedIndexChanged" >
        <items> 
       <asp:ListItem  Text=" "  Selected="false"></asp:ListItem>
       </items>
        </asp:DropDownList>
           
 
     
     
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Kategori_Bilgi" runat="server"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="Yeni_Kategori"  runat="server" onclick="Yeni_Kategori_Click">Yeni Kategori Ekle</asp:LinkButton>
           
 
     
     
        &nbsp; &nbsp;
        <asp:LinkButton ID="Search_Link" runat="server" onclick="Search_Link_Click">Arama Yap</asp:LinkButton>
           
 
     
     
    </asp:Panel>

 
  <asp:Panel ID="Edit_Menu" runat="server">
  <h4>Kategori Açıklaması:</h4>
  <textarea runat="server" ID="Ayrinti" cols="20" name="S1" rows="2"></textarea>&nbsp;&nbsp;
      <asp:Label ID="Mesaj_Bilgisi" runat="server"></asp:Label>
      <br /><br />
  <asp:Button runat="server" id="Duzenle" style="margin-left: 0px" 
          Text="Seçili Kategoriyi Düzenle" onclick="Duzenle_Click" />
  <asp:Button runat="server" id="Sil" Text="Seçili Kategoriyi Sil" 
          onclick="Sil_Click" />
  <asp:Button runat="server"  id="Urunlere_Git" Text="Bu kategorideki Ürünleri Yönet" 
          onclick="Urunlere_Git_Click1"/>
  <asp:Button runat="server"  id="Alt_Kategori_Ekle" 
          Text="Seçili olana alt kategori ekle " onclick="Alt_Kategori_Ekle_Click" 
          style="margin-top: 2px" />

     
      <asp:Label ID="Bilgi_Mesaj" runat="server"></asp:Label>

      <br />
        <br />
          <br />

</asp:Panel>
 
      <asp:LinkButton ID="Tekrar_Yukle" runat="server" onclick="Tekrar_Yukle_Click">LinkButton</asp:LinkButton>

<br />

    <br />
   <br />
    <br />
    <asp:Panel ID="Edit_Screen" runat="server">
        <asp:Label ID="Reg_Info" runat="server"></asp:Label>
        <br />
        <br />
      <asp:Label ID="C_Name" runat="server">Kategori Adı:</asp:Label>
        <asp:TextBox ID="C_Name_Text" runat="server"></asp:TextBox>
      <br /> <br />
        <asp:Label ID="C_Exp" runat="server" Text="Kategori Açıklaması:"></asp:Label>
       
        <asp:TextBox ID="C_Exp_Text" runat="server"></asp:TextBox>
    <br />
    <br />
        <asp:Button ID="C_Register" runat="server" onclick="C_Register_Click" 
            style="width: 61px; margin-left: 0px" Text="Kaydet" />
        &nbsp;<br /> 
        <asp:Button ID="C_Sub_Reg" runat="server" onclick="C_Sub_Reg_Click" 
            Text="Alt Kategoriyi Ekle" />
        <br />
      
        <asp:Button ID="C_Edit_Reg" runat="server" onclick="C_Edit_Reg_Click" 
            Text="Değişiklikleri Kaydet" />
       
        &nbsp;
  
       
        </asp:Panel>
 
 
</asp:Content>

