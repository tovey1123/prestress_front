   M S B A M L   `   `   `  ����K  GPre_stressSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Pre_stressSystemPre_stressSystem  5      !    Pre_stressSystem.detectPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Pre_stressSystem     5        N� Title$  
detectPage��5      .��5	      (�  (� Stretch$
 Fill=�6   .��5
   
   �� $	��768��6   $
��1366��6   .+�5      �� -   5         ��Name$ detectPrestress��5      $��预应力检测��6-    �� Left$	 550��6=   
 �� Top$ 74��6O   $��90��6_   $	��289��6k   $��53(�6w   
�������6�   �� 5      -   5        $	 COM��5      $��
串口号:��6!   $	 129��60   $	 260��6B   $��41��6S   $��90��6_   $��25(�6j   �� 5      -   5         ��Name$ COMList��5      $	 231��6(   $	 266��6:   $	��120��6K   $��27��6W   $��14(�6c   $��Center=�6q   $��Center=�6�   .@�5      �� $
��COM1��6    �� 
IsSelected . 6.   �� 5      $
��COM2��6   �� 5      $
��COM3��6   �� 5      $
��COM4��6   �� 5      $
��COM5��6   �� 5      $
��COM6��6   �� 5      $
��COM7��6   �� 5      $
��COM8��6   �� 5      $
��COM9��6   �� 5      $��COM10��6   5      �� 5      -   5         ��Name$ autoSelectCOM��5      $��自动检测��6,   $	 231��6;   $	 301��6M   $	��120��6^   $��27��6�   $��14(�6�   $��Center=�6�   �� 5       -   5        $ BaudRate��5       $��
波特率:��6&   $	 129��65   $	 335��6G   $��25(�6X   $��90��6f   $��41��6q   �� 5!      -   5        $ BaudRateList��5!      $	 231��6-   $	 348��6?   $	��120��6P   $��27��6\   $��14(�6h   $��Center=�6v   $��Center=�6�   $��Center=�6�   $��Center=�6�   .@�5"      �� $
��1200��6   �� 5#      $
��2400��6   �� 5$      $
��4800��6   �� 5%      $
��9600��6   �� 5&      $��19200��6   �� 5'      $��38400��6    . 6/   5(      �� 5)      -   5        $ DataBit��5)      $��
数据位:��6%   $	 129��64   $	 385��6F   $��25(�6W   $��90��6e   $��41��6p   �� 5*      -   5        $ DataBitList��5*      $	 231��6,   $	 397��6>   $	��120��6O   $��27��6[   $��16(�6g   $��Center=�6u   $��Center=�6�   .@�5+      �� $��6��6   �� 5,      $��7��6   �� 5-      $��8��6    . 6+   5.      �� 5/      -	   5         ��Name$ 
txtReceive��5/      $��91��6,   $	 128��68   $	 527��6K   $	��227��6\   ���	#FFFFF7F7$��16(�6   $
��Wrap=�6�   �� 50      $��10��6   $	 264��6$   $��0q�66   $	 418��6A   $	��450��6R   	 J� RenderTransformOrigin$	 0.5,0.5&�6^   z�51      V� 52      .~�53      �� �� 54      �� 55      
 �� Angle$	
 -90Q�6*   T� 56      57      59      �� 5<      -
   5        $ 	sensor_id��5<      $��传感器ID:��6'   $	 523��68   $	 245��6J   $��21(�6[   �� 5=      -   5        $ txt_ID��5=      $
��Wrap=�6(   $�� ��6<   $	 665��6D   $	 246��6V   $��27��6g   $	��120��6s   
���   �
�������$��18(�6�    �� TextAlignment$ Center=�6�   �� 5>      -   5        $ railway_name��5>      $��线路名称:��6*   $	 799��6:   $	 246��6L   $��18(�6]   �� 5?      -   5        $ txt_railway��5?      $
��Wrap=�6-   $�� ��6A   $	 906��6I   $	 247��6[   $��27��6l   $	��120��6x   
���   �
�������$��13(�6�   $ Center=�6�   �� 5@      -   5        $ 	sensor_SN��5@      $��传感器编号:��6'   $
 1037��68   $	 246��6K   $��18(�6\   �� 5A      -   5        $ txt_SN��5A      $
��Wrap=�6(   $�� ��6<   $
 1145��6D   $	 249��6W   $��27��6h   $	��120��6t   
���   �
�������$��18(�6�   $ Center=�6�   �� 5B      -   5        $ sensor_data��5B      $��预应力数据:��6)   $	 522��6:   $	 339��6L   $��16(�6]   $	��110��6k   �� 5C      -   5        $ txt_data��5C      $
��Wrap=�6*   $�� ��6>   $	 665��6F   $	 340��6X   $��27��6i   $	��120��6u   
���   �
�������$��18(�6�   $ Center=�6�   �� 5D      -   5        $ param��5D      $��转换系数:��6#   $	 802��63   $	 340��6E   $��18(�6V   $��95��6d   �� 5E      -   5        $ 	txt_param��5E      $
��Wrap=�6+   $�� ��6?   $	 906��6G   $	 345��6Y   $��25��6j   $	��120��6v   
���   �
�������$��18(�6�   $ Center=�6�   �� 5F      -   5        $ 	prestress��5F      $��预应力值:��6'   $
 1044��67   $	 337��6J   $��21(�6[   �� 5G      -   5        $ txt_prestress��5G      $
��Wrap=�6/   $�� ��6C   $
 1147��6K   $	 342��6^   $��27��6o   $	��120��6{   
���   �
�������$��18(�6�   $ Center=�6�   �� 5H      -   5        $ status��5H      $��预应力状态:��6$   $	 522��65   $	 430��6G   $��18(�6X   �� 5I      -   5        $ stress_status��5I      $
��Wrap=�6/   $�� ��6C   $	��120��6K   $��27��6W   $	 665��6c   $	 436��6u   
���   �
�������$��18(�6�   $ Center=�6�   �� 5J      -   5        $ sensorstatus��5J      $��传感器状态:��6*   $	 790��6;   $	 434��6M   $��18(�6^   �� 5K      -   5        $ sensor_status��5K      $
��Wrap=�6/   $�� ��6C   $	��120��6K   $��27��6W   $	 904��6c   $	 436��6u   
���   �
�������$ Center=�6�   $��18(�6�   �� 5L      -   5        $ sensor_location��5L      $��传感器位置:��6-   $
 1037��6>   $	 434��6Q   $��18(�6b   �� 5M      -   5        $ txt_location��5M      $
��Wrap=�6.   $�� ��6B   $
 1147��6J   $	 435��6]   $��47��6n   $	��120��6z   
���   �
�������$��19(�6�   �� 5N      -   5        $��bd_analysis��5N      $��0q�6(   $��35��6<   $	 689��6H   $	 565��6Z   $��96��6k    J� Opacity$	 0.8Q�6�   ��5O      �� 5P       �� ImageSource$ picture/button.png��6!   ._�5R      �� $��分析数据��6   $��	19,4,14,6q�6'   5S      �� 5T      -   5        $��bd_save��5T      $��0q�6$   $��35��68   $	 859��6D   $	 565��6V   $��96��6g   $	 0.8Q�6r   ��5U      �� 5V      $ picture/button.png��6!   ._�5X      �� $��保存数据��6   $��20,4,0,6q�6'   $
��Left=�69   $��68��6T   5Y      �� 5Z      -   5         ��Name$ btn_open_close��5Z      $��0q�6-   $��35��6A   $	 133��6M   $	 453��6_   $��86��6p   $	 0.8Q�6�   ��5[      �� 5\      $ picture/button.png��6!   ._�5^      �� -   5        $��label_open_close��5^      $��打开串口��60   $��	15,4,10,6q�6?   5_      �� 5`      -    5        $ bd_save_Copy��5`      $��0q�6+   $��35��6?   $	 268��6K   $	 451��6]   $��87��6n   $	 0.8Q�6y   ��5a      �� 5b      $ picture/button.png��6!   ._�5d      �� $��清空数据��6   $��14,4,0,6q�6'   $
��Left=�69   $��68��6T   5e      5g      5h      5i      