using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UBlockly;
using UnityEngine;
using Input = UBlockly.Input;

[MutatorClass(MutatorId = "object_action_mutator")]
public class ObjectActionMutator : Mutator
{

    private const string JUMP_INPUT_NAME = "JUMP";
    private const string OBJECT_CHECK = "Object";
    
    public bool isJump = false;
    
    public override bool NeedEditor => true;

    public void Mutate(bool isJump)
    {
        if (this.isJump == isJump)
            return;
        this.isJump = isJump;
        if (mBlock != null)
        {
            UpdateInternal();
        }
    }
    
    // 세팅
    public override XmlElement ToXml()
    {
        if (false == isJump)
            return null;
        
        XmlElement xmlElement = XmlUtil.CreateDom("mutation");

        if(isJump)
            xmlElement.SetAttribute("isJump", "true");
        
        return xmlElement;
    }

    // 적용
    public override void FromXml(XmlElement xmlElement)
    {
        isJump = xmlElement.HasAttribute("isJump");
        
        this.UpdateInternal();
    }

    // 모델 변경
    private void UpdateInternal()
    {
        List<Input> oldInputs = new List<Input>(mBlock.InputList);
        List<Input> newInputs = new();
        
        newInputs.Add(oldInputs[0]);   
        oldInputs.RemoveRange(0, 1);
        if (isJump)
        {
            Input inputValue = InputFactory.Create(
                Define.EConnection.InputValue,
                JUMP_INPUT_NAME,
                Define.EAlign.Right,
                new List<string>() { OBJECT_CHECK });
            
            inputValue.AppendField(new FieldLabel("Jump", "JumpAction"));
            
            newInputs.Add(inputValue);
        }
        else
        {
            
        }
        mBlock.Reshape(newInputs);
    }
    
    protected override void OnAttached()
    {
        UpdateInternal();
    }
}
