   M S B A M L   `   `   `  ���� I  ECompressionApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CompressionAppCompressionApp      CompressionApp.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:CompressionApp       � Title$  
MainWindow��$	��550��$	��800��.���  � ColumnDefinitions �� $��1* ��� $��1* ��� $��1* � � RowDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� -   $��textBox��$y�Hidden=�$��0��$��0��$��3��$��2��$��15q�$	��740��$
��Left=�.��The text of the uploaded file�� -    ��Name$ imageSelected��$y�Hidden=�$��0��$��0��$��3��$��2��$��15q�$	��740��$��Center=��� -   $��filePathlbl��$��0��$��2��$��20q�$��3��$
��Left=�.��file path :�� $��0��$��3��.+���  �� Orientation$ 
Horizontal=�$��	20,0,20,0q�.+��� .+��� $
��Left=�.��Size Before the operation :�� $
��Left=�.��Size After the operation :�� $��50��.+��� -   $��
sizeBefore��$
��Left=�.��0 Byte�� -   $��	sizeAfter��$
��Left=�.��0 Byte�� -   $$��Compress Image Using JpegLossy��$��23q�$
��Left=�$��2q�$��30��$	��195��$	��Top=�� $��1��$��2��$��2��$��3�� �� $��1* ��� $��1* �.+��� $��0��$��	0,0,20,10q�$��Right=�$��Bottom=�
�������
�������$	��200��$	��180��$��1q� �� CornerRadius$ 8,8,3,3��._��� .+��� -   $��Decompress File (.fano)��$��15q�$
��Left=�$��2q�$��30��$	��165��$	��Top=��� -   $��Decompress file (.lzw)��$��15q�$
��Left=�$��2q�$��30��$	��165��$	��Top=��� -	   $��Decompress folder ��$��15q�$
��Left=�$��2q�$��30��$	��165��$	��Top=��� $��1��$��	0,0,30,10q�$��Right=�$��Bottom=�
�������
�������$	��195��$	��180��$��1q�$ 8,8,3,3��._��� .+��� -
   $��Compress text file (.txt)��$��15q�$
��Left=�$��2q�$��30��$	��160��$	��Top=��� -   $��Compress image file��$��15q�$
��Left=�$��2q�$��30��$	��160��$	��Top=��� -   $��Compress folder ��$��15q�$
��Left=�$��2q�$��30��$	��160��$	��Top=�