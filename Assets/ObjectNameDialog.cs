using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UBlockly;
using UBlockly.UGUI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNameDialog : BaseDialog
{
    [SerializeField] private Text m_InputLabel;
    [SerializeField] private InputField m_Input;

    private bool mIsRename = false;
        
    private string mOldVarName;
    public void Rename(string varName)
    {
        mOldVarName = varName;
        mIsRename = true;
        m_InputLabel.text = I18n.Get(MsgDefine.RENAME_VARIABLE);
    }

    protected override void OnInit()
    {
        m_InputLabel.text = I18n.Get(MsgDefine.NEW_VARIABLE);
        AddCloseEvent(() =>
        {
            if (mIsRename)
            {
                Manager.Instnace.Rename(mOldVarName, m_Input.text);
                BlocklyUI.WorkspaceView.Workspace.RenameVariable(mOldVarName, m_Input.text,mapType:Workspace.MapType.Object);
            }
            else
            {
                Manager.Instnace.Add(m_Input.text);
                BlocklyUI.WorkspaceView.Workspace.CreateVariable(m_Input.text, mapType:Workspace.MapType.Object);
            }
        });
    }
}
