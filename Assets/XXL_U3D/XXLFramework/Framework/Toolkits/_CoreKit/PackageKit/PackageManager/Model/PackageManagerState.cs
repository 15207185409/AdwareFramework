/****************************************************************************
 * Copyright (c) 2020.10 
 * 
 * https://xxlframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{
    internal class PackageManagerState
    {
        public static BindableProperty<List<PackageRepository>> PackageRepositories =
            new BindableProperty<List<PackageRepository>>(new List<PackageRepository>());
        
        public static BindableProperty<int> CategoryIndex = new BindableProperty<int>(0);
        
        public static BindableProperty<List<string>> Categories = new BindableProperty<List<string>>();
        
        public static BindableProperty<int> AccessRightIndex = new BindableProperty<int>(0);
        
        public static BindableProperty<string> SearchKey = new BindableProperty<string>("");
    }
}