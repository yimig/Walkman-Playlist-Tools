   M S B A M L   `   `   `  ����Q  MWalkman Playlist Tools, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Walkman_Playlist_ToolsWalkman_Playlist_Tools  5   	   '    !Walkman_Playlist_Tools.PlayWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Walkman_Playlist_Tools     5   	   -   5          � Title$  试听��5   	   $	��200��6   $	��400��6!    � 
ResizeMode$ NoResize=�6-   
������� � Icon$ Icon/play.ico��6Z   .��5	      �  � RowDefinitions 5
   
   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   .+�5   
   �� $
��Auto=�6   $��4��6:   $
��10,0q�6N   .��5   
   �� -   5        $��InfoList��5      $
��Wrap=�6%   5      5      �� 5   
   $��1��6   $��4��6"    �� Orientation$ 
Horizontal=�67   .+�5      �� -   5        $��NowBox��5   m   $��30��6   $��10,0,0,0q�6#   $��Center=�65   $��Center=�6R   0:06�   �� 5      -   5        $��	TimeTrack��5   I   $	��6,5q�6   $��Center=�6"   $	��300��6=   6\   �� 5      -   5        $��	LengthBox��5   [   $��40��6   $��Center=�6#   $��Center=�6@   6n   5      �� 5   
   -   5        $��
BackButton��5   d   $��2��6   $��0��6   $��20,10q�6/   ���	#FFEEF5FD � ToolTip$ 
快退5秒��6U   .��5      �� $��Icon/back.ico��6   6-   5       �� 5!   
   -   5        $��
StopButton��5!   D   $��2��6   $��1��6   $��1��60   $��20,10q�6V   ���	#FFEEF5FD$ 暂停/播放��6|   .��5"      �� -   5        $��
stopButton��5"   ,   $��Icon/pause.ico��6   6@   5#      �� 5$   
   -	   5        $��PositionButton��5$   D   $��2��6   $��2��6   $��1��6/   $��20,10q�6Z   ���	#FFEEF5FD$* $定位到歌曲四分之一的位置��6�   .��5%      �� $��Icon/locate.ico��6   6/   5&      �� 5'      -
   5        $��
SkipButton��5'   h   $��2��6   $��3��6#   $��20,10q�63   ���	#FFEEF5FD$ 
快进5秒��6Y   .��5(      �� $��Icon/skip.ico��6   61   5)      5*      5+      