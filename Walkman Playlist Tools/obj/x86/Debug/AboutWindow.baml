   M S B A M L   `   `   `  ����Q  MWalkman Playlist Tools, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Walkman_Playlist_ToolsWalkman_Playlist_Tools  5   	   (    "Walkman_Playlist_Tools.AboutWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Walkman_Playlist_Tools     5   	     � Title$  关于��5   	   $	��600��6   $	��400��6!    � WindowStartupLocation$ CenterScreen=�6-    � 
ResizeMode$ NoResize=�6R    � Icon$ Icon/aboutwindow.ico��6h   .��5	      � .+�5
   
   �� -   5        $��AboutControl��5
   !   $��10q�6   .@�5      �� $��关于��6   .��5      �� $��10q�6   .+�5      �� $��版本信息��6   .��5      �� .+�5      �� $
��Bold"�6(   $��15(�6:   $��Center=�6H   $��5q�6c   $��Center=�6n   Ver 1.0.1.06�   �� 5      Build 2019/2/668   �� 5       �� Orientation$ 
Horizontal=�6)   $��RightToLeft=�6B   .+�5   "   �� -   5        $��CheckUpdateButton��5   )   $	��100��6B   $��25��6N   $��5q�6Z   .��6�   检查新版本6�   5      5      5      �� 5      $��Apache License��6   $��0,10,0,0q�67   .��5      �� $
��Wrap=�6$   $��5q�68   ��Copyright [2019] [upane.net] Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.5%      5&      5'      5(      �� 5)      $	��Q&A��6   .��5*      �� .��5+      �� $��
10,10,0,10q�6!   .+�5,      �� $��15(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��0,5q�6X   dbQ：既然Music Center/批处理脚本能够制作播放列表，那还用这个软件做什么？6�   �� 5-      $
��Wrap=�6$   ��A：Music Center每次导入播放列表都需要导入音乐文件，如果多个播放列表中有相同的曲目该曲目则会反复导入，不仅损害机器闪存的寿命还造成时间上的浪费。比起使用批处理脚本，本软件能提供用户友好的界面和更加强大的管理功能。6�   �� 5.      $��15(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��0,5q�6X   NLQ：读取音乐信息有什么用？这个不是只能做播放列表吗？6�   �� 5/      $
��Wrap=�6$   ��A：读取音乐信息后可以使用自动归类的功能，另外也方便用户选择音乐。虽然目前看起来用处并不大，但是将来希望能够集成补充音乐信息的功能，从某种角度讲应该是为未来的拓展升级做准备吧。6�   �� 50      $��15(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��0,5q�6X   NLQ：语言识别的精度太差了，有什么办法可以让它精确些？6�   �� 51      $
��Wrap=�6$   ��A：语言识别的原理是利用各国语言的特征进行识别的，也就是说如果特性不明显机器就难以判断语种，进而导致出错。增加输入样本量可以提高检测准确度。本软件可对歌曲的标题和歌词（如果本地有的话）进行语言识别，仅有标题的话数据量实在太少，最好添加上歌词试试（当然是本国语言，不能翻译）。6�   �� 52      $��15(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��0,5q�6X   *(Q：为什么调用开机启动权限？6u   �� 53      $
��Wrap=�6$   KIA：我也纳闷怎么就调用了开机启动权限，360出来挨打！6U   �� 54      $��15(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��0,5q�6X   ^\Q：win7在点击某些打开超链接的控件时崩溃？（比如前往更新地址等）6�   �� 55      $
��Wrap=�6$   ��A：这个程序中调用浏览器打开网页是用命令行实现的，虽然在win10中没问题，但是这个命令在win7中好像需要管理员权限才能执行，没有就会崩溃。您可以手动打开链接（项目页面地址在获取支持页面）或者使用管理员权限打开这个程序。6�   �� 58      $ 
Horizontal=�6%   $
��0,20q�6>   .+�59      �� $��15(�6(   $
��Bold"�66   $
��Wrap=�6H   $��Center=�6\    不知道如何操作？点击6�   �� 5:      -   5        $��TeachButton��5:   %   $��50��68   $��3q�6C   .��6j   这里6n   �� 5;      $��15(�6(   $
��Bold"�66   $
��Wrap=�6H   $��Center=�6\   查看在线教程6   5<      5>      5?      5@      �� 5A      $��获取支持��6   .��5B      �� $��10q�6   .+�5C      �� $��	Hey Here!��6   .��5D      �� $��5q�6%   .+�5E      �� $
��Wrap=�6(   ��该程序侵犯了您的权益？运行出现了问题？有十分需要的功能或新的想法？您可以访问这个软件的项目页进行留言，我将及时答复：6x   �� 5F      -   5        $��
ProjectURL��5F   :   
����  �http://upane.cn/archives/5096�   �� 5G      $
��Wrap=�6(   VT该程序将在近期开源，如果需要源码，可以关注我的Github账户：6^   �� 5H      -   5        $��GitURL��5H   :   
����  �https://github.com/yimig6�   �� 5I      $
��Wrap=�6(   YW另外，您也可以关注我的个人博客，偶尔会分享些写代码的心得：6[   �� 5J      -   5        $��BlogURL��5J   :   
����  �http://upane.cn6�   �� 5K      20菜鸟一只，还望各路大佬还望轻喷。6:   �� 5L      
2019/2/862   5M      5N      5O      5P      5Q      5R      5S      