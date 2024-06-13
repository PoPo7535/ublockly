/****************************************************************************

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


using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace UBlockly
{
    /// <summary>
    /// ifelse ifelse 블록에 대한 뮤테이터. 이 클래스는 블록 모델을 수정하지만 보기 계층 구조를 업데이트하거나 사용자에게 편집기를 표시하는 일을 담당하지 않습니다.
    /// </summary>
    [MutatorClass(MutatorId = "controls_if_mutator")]
    public class IfElseMutator : Mutator
    {
        private const string ELSE_INPUT_NAME = "ELSE";
        private const string IF_INPUT_PREFIX = "IF";
        private const string DO_INPUT_PREFIX = "DO";
        private const string CHECK = "Boolean";
        private const Define.EAlign ALIGN = Define.EAlign.Right;
        
        /// <summary>
        /// 이 블록에 대한 else if 입력의 수입니다.
        /// </summary>
        private int mElseIfCount = 0;
        public int ElseIfCount { get { return mElseIfCount; } }
        
        /// <summary>
        /// 이 블록 끝에 else 문이 있으면 참이고 그렇지 않으면 거짓입니다
        /// </summary>
        private bool mHasElse = false;
        public bool HasElse { get { return mHasElse; } }

        public override bool NeedEditor
        {
            get { return true; }
        }
        
        /// <summary>
        /// Mutator를 제공된 값으로 업데이트하여 프로그래밍 방식으로 돌연변이 이벤트를 호출하는 편리한 방법입니다.
        /// </summary>
        /// <param name="elseIfCount">The number of else if inputs for this block.</param>
        /// <param name="hasElse">True if this block should have a final else statement.</param>
        public void Mutate(int elseIfCount, bool hasElse)
        {
            if (elseIfCount == mElseIfCount && hasElse == mHasElse)
                return;

            mElseIfCount = elseIfCount;
            mHasElse = hasElse;
            if (mBlock != null)
            {
                UpdateInternal();
            }
        }

        protected override void OnAttached()
        {
            UpdateInternal();
        }
        
        public override XmlElement ToXml()
        {
            if (mElseIfCount <= 0 && !mHasElse)
                return null;
            
            XmlElement xmlElement = XmlUtil.CreateDom("mutation");
            if (mElseIfCount > 0)
                xmlElement.SetAttribute("elseif", mElseIfCount.ToString());

            if (mHasElse)
                xmlElement.SetAttribute("else", "1");
            
            return xmlElement;
        }

        public override void FromXml(XmlElement xmlElement)
        {
            mElseIfCount = 0;
            mHasElse = false;
            if (xmlElement.HasAttribute("elseif"))
                mElseIfCount = int.Parse(xmlElement.GetAttribute("elseif"));
            if (xmlElement.HasAttribute("else"))
                mHasElse = true;
            
            this.UpdateInternal();
        }

        /// <summary>
        /// 지정된 개수에 대해 모델 변경을 수행합니다. 가능한 한 많은 입력을 재사용합니다.,
        /// 필요한 경우 새 입력을 만듭니다. 남은 입력은 연결이 끊어지고 버려집니다..
        /// </summary>
        private void UpdateInternal()
        {
            List<Input> oldInputs = new List<Input>(mBlock.InputList);
            List<Input> newInputs = new List<Input>();
            
            // 끝을 위해 else 입력을 따로 둡니다.
            Input elseInput = mBlock.GetInput(ELSE_INPUT_NAME);
            if (elseInput != null)
                oldInputs.Remove(elseInput);
            
            // 첫 번째 ifdo 블록을 새 입력 사전으로 이동
            newInputs.Add(oldInputs[0]);    //IF0
            newInputs.Add(oldInputs[1]);    //DO0
            oldInputs.RemoveRange(0, 2);
            
            // 기존 입력이 있으면 복사하고, 없으면 새 입력을 만듭니다.
            for (int i = 1; i <= mElseIfCount; i++)
            {
                if (oldInputs.Count >= 2)
                {
                    newInputs.Add(oldInputs[0]);    //IFi
                    newInputs.Add(oldInputs[1]);    //DOi
                    oldInputs.RemoveRange(0, 2);
                }
                else
                {
                    // IFi 값 입력
                    Input inputValue = InputFactory.Create(Define.EConnection.InputValue, IF_INPUT_PREFIX + i, ALIGN, new List<string>() {CHECK});
                    inputValue.AppendField(new FieldLabel(null, I18n.Get(MsgDefine.CONTROLS_IF_MSG_ELSEIF)));

                    // DOi 문 입력
                    Input inputStatement = InputFactory.Create(Define.EConnection.NextStatement, DO_INPUT_PREFIX + i, ALIGN, null);
                    inputStatement.AppendField(new FieldLabel(null, I18n.Get(MsgDefine.CONTROLS_IF_MSG_THEN)));
                    
                    newInputs.Add(inputValue);
                    newInputs.Add(inputStatement);
                }
            }
            
            // 필요한 경우 else 절을 추가하십시오.
            if (mHasElse)
            {
                if (elseInput == null)
                {
                    elseInput = InputFactory.Create(Define.EConnection.NextStatement, ELSE_INPUT_NAME, ALIGN, null);
                    elseInput.AppendField(new FieldLabel(null, I18n.Get(MsgDefine.CONTROLS_IF_MSG_ELSE)));
                }
                newInputs.Add(elseInput);
            }
            else if (elseInput != null)
            {
                // else 문을 폐기하십시오
                elseInput.Dispose();
            }

            // 추가 입력 정리
            foreach (Input input in oldInputs)
                input.Dispose();
            oldInputs.Clear();
            
            mBlock.Reshape(newInputs);
        }
    }
}
