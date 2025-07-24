/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
namespace XXLFramework
{
    public interface ICodeGenTemplate
    {
        CodeGenTask CreateTask(BindGroup bindGroup);
    }
}
#endif