   M S B A M L   `   `   `  ����C  ?serverHW, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:serverHWserverHW  5   	       serverHW.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:serverHW     5   	     � Title$  Server��5   	   $	��617��6   $	��800��6%   .��5	      � .+�5
   
   �� $��Socket Address: ��6   $
��Left=�6+   $��	10,10,0,0q�6F   $	��Top=�6Y   $��43��6q   $	��180��6}   $��22(�6�   �� 5   
   $��Port: ��6   $
��Left=�6!   $��
360,10,0,0q�6<   $	��Top=�6P   $��43��6h   $��61��6t   $��22(�6   �� 5   
   -   5         ��Name$ 	ipContent��5      $
��Left=�6%   $��38��6@   $��
195,10,0,0q�6L    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��160��6�   $��24(�6�   $��	127.0.0.1��6�   �� 5   
   -   5        $ portContent��5      $
��Left=�6'   $��38��6B   $��
421,10,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��154��6�   $��24(�6�   $��20001��6�   �� 5   
   -   5        $ 
messageAll��5      $
��Left=�6&   $	��335��6A   $��
10,201,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��773��6�   �� 5   
   -   5         ��Name$ startBtn��5      $��Start Listen��6#   $
��Left=�6:   $��
609,15,0,0q�6U   $	��Top=�6i   $	��159��6�   $��24(�6�   $��48��6�   6�   �� 5   
   -   5        $ stopBtn��5      $��Stop Listen��6"   $
��Left=�68   $��
609,68,0,0q�6S   $	��Top=�6g   $	��159��6   $��24(�6�   $��48��6�   6�   �� 5   
   $��	Message: ��6   $
��Left=�6$   $��
10,139,0,0q�6?   $	��Top=�6S   $��43��6k   $	��107��6w   $��22(�6�   �� 5   
   -   5        $ messageContent��5      $
��Left=�6*   $��38��6E   $��122,139,0,0q�6Q   $
 Wrap=�6f   $	��Top=�6z   $	��453��6�   $��24(�6�   �� 5   
   -   5        $ sendBtn��5      $
��Send��6"   $
��Left=�61   $��629,129,0,0q�6L   $	��Top=�6a   $	��123��6y   $��24(�6�   $��48��6�   6�   5      5      