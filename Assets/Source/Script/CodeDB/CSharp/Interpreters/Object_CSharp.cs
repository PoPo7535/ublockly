/****************************************************************************

Functions for interpreting c# code for blocks.

Copyright 2016 sophieml1989@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

****************************************************************************/


using System.Collections;
using UnityEngine;

namespace UBlockly
{
    [CodeInterpreter(BlockType = "object_get")]
    public class Object_Get_Cmdtor : ValueCmdtor
    {
        protected override DataStruct Execute(Block block)
        {
            // Block는 input 혹은 이전 블럭의 데이터를 가지고 있다.

            // GetName을 통해 데이터를 가져온다.
            // 데이터를 반환
            string value = block.GetFieldValue("OBJECT");
            return new DataStruct(value);
        }
    }

    [CodeInterpreter(BlockType = "object_jump")]
    public class Object_Jump_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator ctor0 = CSharp.Interpreter.ValueReturn(block, "OBJECT");
            yield return ctor0;

            CmdEnumerator ctor1 = CSharp.Interpreter.ValueReturn(block, "VAR");
            yield return ctor1;
            
            var objName = ctor0.Data.StringValue;
            var power = ctor1.Data.NumberValue.Value;
            
            Manager.Instnace.GetObject(objName).Jump(power);

            // Number result = CSharp.VariableDatas.GetData(tmp).NumberValue + arg1.NumberValue;
            // // SetData는 다음 블럭으로 반환하는 데이터다.
            // CSharp.VariableDatas.SetData(tmp, new DataStruct(result));
        }
    }

    [CodeInterpreter(BlockType = "object_turn")]
    public class Object_Turn_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator objectCtor = CSharp.Interpreter.ValueReturn(block, "OBJECT");
            yield return objectCtor;
            var objName = objectCtor.Data.StringValue;

            CmdEnumerator angleCtor = CSharp.Interpreter.ValueReturn(block, "ANGLE");
            yield return angleCtor;
            var angle = angleCtor.Data.NumberValue.Value;
            
            CmdEnumerator valueCtor = CSharp.Interpreter.ValueReturn(block, "VALUE");
            yield return valueCtor;
            var value = valueCtor.Data.NumberValue.Value;
            
            var dir = block.GetFieldValue("DIRECTION");
            var mode = block.GetFieldValue("MODE");

            bool check = false;
            Manager.Instnace.GetObject(objName).Trun(dir, mode, angle, value, () => check = true);
            
            yield return new WaitUntil(()=> check);
        }
    }
    
    [CodeInterpreter(BlockType = "object_actions")]
    public class Object_Actions_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator objectCtor = CSharp.Interpreter.ValueReturn(block, "OBJECT");
            yield return objectCtor;
            var objName = objectCtor.Data.StringValue;

            CmdEnumerator angleCtor = CSharp.Interpreter.ValueReturn(block, "ANGLE");
            yield return angleCtor;
            var angle = angleCtor.Data.NumberValue.Value;
            
            CmdEnumerator valueCtor = CSharp.Interpreter.ValueReturn(block, "VALUE");
            yield return valueCtor;
            var value = valueCtor.Data.NumberValue.Value;
            
            var dir = block.GetFieldValue("DIRECTION");
            var mode = block.GetFieldValue("MODE");

            bool check = false;
            Manager.Instnace.GetObject(objName).Trun(dir, mode, angle, value, () => check = true);
            
            yield return new WaitUntil(()=> check);
        }
    }

    [CodeInterpreter(BlockType = "object_move")]
    public class Object_Move_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
            CmdEnumerator objectCtor = CSharp.Interpreter.ValueReturn(block, "OBJECT");
            yield return objectCtor;
            var objName = objectCtor.Data.StringValue;

            CmdEnumerator x = CSharp.Interpreter.ValueReturn(block, "X");
            yield return x;
            var X = x.Data.NumberValue.Value;
            
            CmdEnumerator y = CSharp.Interpreter.ValueReturn(block, "Y");
            yield return y;
            var Y = y.Data.NumberValue.Value;
            
            CmdEnumerator z = CSharp.Interpreter.ValueReturn(block, "Z");
            yield return z;
            var Z = z.Data.NumberValue.Value;
            
            var mode = block.GetFieldValue("MODE");

            var obj = Manager.Instnace.GetObject(objName);

            switch (mode)
            {
                case "POSITION":
                    obj.transform.Translate(new Vector3(X, Y, Z));
                    break;
                case "ROTATION":
                    obj.transform.Rotate(Quaternion.Euler(new Vector3(X, Y, Z)).eulerAngles);
                    break;
                case "SCALE":
                    obj.transform.localScale = (new Vector3(X, Y, Z));
                    break;
            }
        }
    }
}
