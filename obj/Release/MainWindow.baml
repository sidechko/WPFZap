   M S B A M L   `   `   `  ���� ^  ZЗаписной календарь, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPFkalendarZpisnoyWPFkalendarZpisnoy  #    WPFkalendarZpisnoy.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:WPFkalendarZpisnoy       � Title$  
MainWindow��$	��450��$	��800�� � 	Resources    
moveButton �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value  	dayButton	 ]� 
Conditions
 �� Property �� Value��%`  &	        &	     �� " ��.���  � �� �	#FFB6B6B6��  � �� �� " ��.R��� #��z��$��2q�._��� #��z��$��Center=�$��Center=�#��z��$��0,0,0,5q�$
��Bold"� P�  �@J�IsMouseOver .  . ���  � �� �	#FFCDCDCDP�  � �� . . ���  � ��
 ������� " ��.���  � �� �	#FFDEECEC��  � �� �� " ��.R��� #��z��$��2q�._��� #��z��$	��Top=�$
��Left=�$��14(�$��3,3,0,5q� P�  �@�Tag$ T��. ���  � �� �	#FFE9DEEC]� 	 �� 
 �@�Tag$ T���� 
 �@J�IsMouseOver .  ./���  � �� �	#FFCFB9DC]� 	 �� 
 �@�Tag$ T���� 
 � �� . ./���  � �� �	#FFF7D7F9P�  �@�Tag$ N��. ���  � �� �	#FFDEECDF]� 	 �� 
 �@�Tag$ N���� 
 �@J�IsMouseOver .  ./���  � �� �	#FFBEDCB9]� 	 �� 
 �@�Tag$ N���� 
 � �� . ./���  � �� �	#FFDAF9D7P�  �@�Tag$ W��. ���  � �� �	#FFECDEDE]� 	 �� 
 �@�Tag$ W���� 
 �@J�IsMouseOver .  ./���  � �� �	#FFDCB9B9]� 	 �� 
 �@�Tag$ W���� 
 � �� . ./���  � �� �	#FFF9D7D7]� 	 �� 
 �@�Tag$ O���� 
 �@J�IsMouseOver .  ./���  � �� �	#FFB9DCDB]� 	 �� 
 �@�Tag$ O���� 
 � �� . ./���  � �� �	#FFD7F9F8.���  � RowDefinitions �� $��0.05* ��� .+�o� .@�m� -   $*��$Список всех записей��� $��1�� � ColumnDefinitions �� $��2* ��� $��* �.+�� $��0�� �� $��* ��� $��6* �.+�� $��0��.+��  �� �� $
��2.5* ��� �� �� $
��2.5* ��� .+��� -    ��Name$ yearAdd��$��0��#���   
 � Tag$ Y��$��-��$��30(��� -    ��Name$ SelectedYearLabel��$��1��$��20(�$
��Bold"�$��Center=�$��Center=��� -   $ yearRem��$��2��#���   $ Y��$��+��$��30(��� -   $ mountAdd��$��3��#���   $ M��$��-��$��30(��� -   $ SelectedMountLabel��$��4��$��20(�$
��Bold"�$��Center=�$��Center=��� -   $ mountRem��$��5��#���   $ M��$��+��$��30(�� -   $��daysGrid��$��1�� �� �� �� �� �� �� ��  �� �� �� �� �� � $��1��.+��  �� �� $��7* �.+��� -	   $ selectedDate��$��Не указано��$��20(�$��Center=�$��Center=�� $��1�� �� $��5* ��� $��* ��� $��5* ��� $��* ��� $��* ��� $��* �.+��� -
    ��Name$ selectedDayNotes��$��0��$��0,0,0,10q��� -    ��Name$ noteName��$��1���� -   $ noteTextBody��$��2�� �� TextWrapping$
 Wrap=� �� AcceptsReturn . $ �� HorizontalScrollBarVisibility$ Disabled=�" �� VerticalScrollBarVisibility$
 Auto=�$��0,0,0,10q��� -   $ addNote��$��3��$#��Добавить запись��$	��120��#���   �� -   $ 
changeNote��$��4��$#��Изменить запись��$	��120��#���   �� -   $ 
removeNote��$��5��$!��Удалить запись��$	��120��#���   