using System.Collections;
using System.Collections.Generic;
using UBlockly;
using UBlockly.UGUI;
using UnityEngine;
using UnityEngine.UI;


public class FieldObjectView : FieldVariableView
{
    private MemorySafeVariableObserver mObserver;

    protected override void RegisterTouchEvent()
    {
        m_BtnLabel.onClick.AddListener(() =>
        {
            mMenuGroup.SetActive(true);
        });

        m_BtnSelect.onClick.AddListener(() =>
        {
            mMenuGroup.SetActive(false);
            DialogFactory.CreateFieldDialog<FieldDropdownDialog>(mField);
        });
            
        m_BtnRename.onClick.AddListener(() =>
        {
            mMenuGroup.SetActive(false);
            //pop a rename panel
            ObjectNameDialog dialog = DialogFactory.CreateDialog("object_name") as ObjectNameDialog;
            dialog.Rename(m_Label.text);
        });

        m_BtnDelete.onClick.AddListener(() =>
        {
            mMenuGroup.SetActive(false);
            Manager.Instnace.Delete(m_Label.text);
            mField.SourceBlock.Workspace.DeleteVariable(m_Label.text, Workspace.MapType.Object);
        });
    }

    protected override void OnBindModel()
    {
        m_Label.text = mFieldVar.GetText();
        m_BtnSelect.GetComponentInChildren<Text>().text = I18n.Get(MsgDefine.SELECT_VARIABLE);
        m_BtnRename.GetComponentInChildren<Text>().text = I18n.Get(MsgDefine.RENAME_VARIABLE);
        m_BtnDelete.GetComponentInChildren<Text>().text = I18n.Get(MsgDefine.DELETE_VARIABLE);
        UpdateMenuWidth();
            
        mObserver = new MemorySafeVariableObserver(this);
        mField.SourceBlock.Workspace.ObjectMap.AddObserver(mObserver);
    }
    
    protected override void OnUnBindModel()
    {
        BlocklyUI.WorkspaceView.Workspace.ObjectMap.RemoveObserver(mObserver);
    }

    private void OnVariableUpdate(VariableUpdateData updateData)
    {
        string oldValue = m_Label.text;
        bool updateThis = updateData.VarName.Equals(oldValue);
            
        switch (updateData.Type)
        {
            case VariableUpdateData.Delete:
            {
                if (updateThis)
                {
                    // dispose the block
                    if (!mSourceBlockView.InToolbox)
                        mSourceBlockView.Dispose();
                }
                break;
            }
            case VariableUpdateData.Rename:
            {
                if (updateThis)
                {
                    if (!mSourceBlockView.InToolbox ||
                        BlocklyUI.WorkspaceView.Toolbox.GetCategoryNameOfBlockView(mSourceBlockView) == Define.OBJECT_CATEGORY_NAME)
                    {
                        m_Label.text = updateData.NewVarName;
                        UpdateLayout(XY);
                    }
                }
                break;
            }
        }
    }

    protected new class MemorySafeVariableObserver : IObserver<VariableUpdateData>
    {
        private FieldObjectView mViewRef;

        public MemorySafeVariableObserver(FieldObjectView viewRef)
        {
            mViewRef = viewRef;
        }
            
        public void OnUpdated(object variableMap, VariableUpdateData args)
        {
            if (mViewRef == null || mViewRef.ViewTransform == null)
                ((VariableMap) variableMap).RemoveObserver(this);
            else
                mViewRef.OnVariableUpdate(args);
        }
    }
    
}
