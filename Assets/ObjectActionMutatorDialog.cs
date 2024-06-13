using System.Collections;
using System.Collections.Generic;
using UBlockly.UGUI;
using UnityEngine;
using UnityEngine.UI;

public class ObjectActionMutatorDialog : BaseDialog
{
    [SerializeField] private Toggle toogleJump;
    
    private ObjectActionMutator objectActionMutator => mBlock.Mutator as ObjectActionMutator;
    protected override void OnInit()
    {
        toogleJump.isOn = toogleJump;
            
        AddCloseEvent(() =>
        {
            objectActionMutator.Mutate(toogleJump.isOn);
        });
        
    }
    
}
