   M S B A M L   `   `   `  ����Q  MWalkman Playlist Tools, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Walkman_Playlist_ToolsWalkman_Playlist_Tools  5   	   &     Walkman_Playlist_Tools.DelWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Walkman_Playlist_Tools     5   	   -   5          � Title$  删除选项��5   	   $	��150��6   $	��300��6#    � WindowStartupLocation$ CenterScreen=�6/    � 
ResizeMode$ NoResize=�6T    � Icon$ Icon/deleteplaylist.ico��6j   .��5	      �  � RowDefinitions 5
   
   �� 5      6   �� 5      6   �� 5      6   .+�5   
   	� -   5        $��DelInProgram��5   #   $��0��6    	� 	GroupName$ 
DelOpinion��67   $��
10,10,10,4q�6N   $0��*仅从程序中移除，不删除源文件��6b   6�   	� 5      -   5        $��	DelInFile��5   '   $��1��6   $ 
DelOpinion��68   $
��10,0q�6O   $-��'从程序中删除，且删除源文件��6]   6�   � 5   
   $��2��6    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5      �� -   5        $��Conform��5      $��确定��6$   $
��10,5q�61   6Y   �� 5      -   5        $��Cancel��5      $��取消��6#   $
��10,5q�60   $��1��6>   6g   5      5      5      