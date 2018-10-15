using FairyGUI;
using FairyGUI.Utils;

public partial class MessageBoxView : IFairyGUIView<GComponent>
{
	public GComponent m_UI {get;set;}
	public GTextField m_lbContent;
	public GTextField m_lbTitle;
	public GComponent m_btnClose;
	public GComponent m_btnOk;

	public void BindUI() {
		m_lbContent = (GTextField)m_UI.GetChild("lbContent");
		m_lbTitle = (GTextField)m_UI.GetChild("lbTitle");
		m_btnClose = (GComponent)m_UI.GetChild("btnClose");
		m_btnClose.onClick = onbtnCloseClick;
		m_btnOk = (GComponent)m_UI.GetChild("btnOk");
		m_btnOk.onClick = onbtnOkClick;

	}

	public GComponent CreateView(){
		Window win = new Window();
		win.contentPane =UIPackage.CreateObject("Common", "W_MessageBox").asCom;
		win.Show();
		return win.contentPane;
	}
}