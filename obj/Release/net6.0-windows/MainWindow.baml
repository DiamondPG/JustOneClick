   M S B A M L   `   `   `  ���� I  EJust One Click, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Just_One_ClickJust_One_Click      Just_One_Click.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:Just_One_Click       � 
ResizeMode$  CanMinimize=� � Title$ Just One Click��$	��470��$	��810�� � Icon$ 	/icon.ico�� � 	Resources '   !Just_One_Click.PathColorConverter   PathColorConverter/   )Just_One_Click.IconToImageSourceConverter  IconToImageConverter��%   &	        &	          .��� ���#28282b � ColumnDefinitions �� $��20 ��� $
��auto ��� $
��auto ��� $	��155 ��� $	��155 ��� $��* ��� $��20 � � RowDefinitions �� $��20 ��� $��50 ��� $	��300 ��� $��50 ��� $��20 �.+��� ���#1e1f21$	��410��$��1��$��2��$��1���� $��20(�$
��Bold"�$��Center=�$��3��$��2��$��1��
�������$��Just One Click���� -   $��	LaunchBTN�����#1e1f21$��2��
�������$��20(�$��5,0,5,0q�$��Launch��$��3��$��3��$	��300��$��50��  �� 
TargetType �� Property �� Value��%:   '��       �� " ��.���  �@��CornerRadius$ 15���� -   	 ��Name$	 SettingsBTN�����#1e1f21$��0,0,0,5q�
�������$��2��$��25��$��75��$��Bottom=�$��15(�$��2��$��3��$��Settings�� ��%:   '��       �� " ��.���  �@��CornerRadius$ 10���� -   
 ��Name$
 Profiles�� �� SelectionMode$ Single=����#1e1f21
�������$��1��$	��220��$��1��$��3��$��False������    ListBoxItemStyle �� 
TargetType ��Name J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName��%�  '��       &	 0     �� " ��.���  �@��CornerRadius$ 15���� " ��.���  � ��$ 5q���  � �� �� " ��.R��� $ Bd��
������ 
������ $��1q�$
 True��._���  P�  �@��
IsSelected$
 True��. ��� $ Bd�� � �� �#2D2D30�� $ Bd�� � ��
 ������� $ Bd�� � ��$ 2q��� $ Bd�� �@��CornerRadius$ 10,10,10,10��P�  �@��
IsSelected$ False��. ��� $ Bd�� �@��CornerRadius$ 15������ .R���  �� Orientation$ 
Horizontal=�.+��� .+��� $��30��$��5,0,0,0q�
�������$��18(����� *Name+���� " �� �� BasedOn# [  � ContextMenu �� .@�m� -   $��Add New Profile�� m� Icon �� $��16��$��16��$��images/profile.png���� m� -   $��Rename Item�� �� $��16��$��16��$��images/rename.png��m� -   $��Change Path�� �� m� $��Change Launch Delay�� �� �� m� $��	Duplicate�� �� $��16��$��16��$��images/duplicate.png��m� -   $��Delete�� �� $��16��$��16��$��images/delete.png���� -   $��Console��$��2��$��Console��$��5,0,5,35q�
�������$��3��$��10(�$��2��$
��True���� -	   $
 AppsLB�����#1e1f21
�������$��5��$	��220��$��1��$��3��$��False������  �� ElementName$ Profiles�� �� Path$ SelectedItem.Applications�   SeparatorStyle��%<  '��       &	 0     &	 �     �� " ��.���  �@��CornerRadius$ 15���� " ��.���  � �� �#2D2D30��  � ��$ 1����  � ��$ 0,5,0,5q��� " ��.���  � ��$ 5q���  � �� �� " ��.R��� $ Bd��
������ 
������ $��1q�$
 True��._���  P�  �@��
IsSelected$
 True��. ��� $ Bd�� � �� �#2D2D30�� $ Bd�� � ��
 ������� $ Bd�� � ��$ 2q��� $ Bd�� �@��CornerRadius$ 10,10,10,10��P�  �@��
IsSelected$ False��. ��� $ Bd�� �@��CornerRadius$ 15������ .R��  �� $��32 ��� $	��178 �.+��� $��32��$��Center=�$��32��$��0,0,5,0q����� *	Favicon+ �� 	Converter# [ �� $��1�� �� TextAlignment$ Right=�$��30��$��0,0,5,0q�
�������$��18(����� *Name+�� $��PathTXT��$��1��$��Bottom=�$��0,0,5,0q�$ Right=�$��8(����� *Path+���� *Path+# [  �� #��[ $��0,5,0,5q����� " ��# [  �� .@�m� -
   $��Add New Item (.exe)�� �� $��16��$��16��$��images/add.png��m� $��Add New Browser Item�� �� $��16��$��16��$��images/globe.png��m� $��Add New Text Document�� �� $��16��$��16��$��images/text.png���� m� -   $��Rename Item�� �� $��16��$��16��$��images/rename.png��m� -   $��Change Path�� �� m� -   $��Change Favicon�� �� m� $��Change Launch Delay�� �� $��16��$��16��$��images/timer.png���� m� $��	Duplicate�� �� $��16��$��16��$��images/duplicate.png��m� -   $��Delete�� �� $��16��$��16��$��images/delete.png��