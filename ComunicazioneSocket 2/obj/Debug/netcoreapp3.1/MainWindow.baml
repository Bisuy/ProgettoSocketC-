   M S B A M L   `   `   `  ����O  KComunicazioneSocket2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ComunicazioneSocketComunicazioneSocket  5   	   $    ComunicazioneSocket.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:ComunicazioneSocket     5   	   -   5          �Name$	  SCF��5   	    � Title$ SassoCartaForbiciUtenteB��5   	   $	��450��6*   $	��800��67    � 
ResizeMode$ NoResize=�6C   .��5	      � -   5         �Name$ Lancio_Dei_Dati_Utente_B��5	       � ColumnDefinitions 5
   
   �� 5      .+�5   
   �� -   5         ��Name$ txtIpAdd��5      $
��Left=�6$   $��30��6?   $��	66,55,0,0q�6K    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $	��226��6�   $��	127.0.0.1��6�   �� 5   
   -   5        $ txtDestPort��5      $
��Left=�6'   $��30��6B   $��
317,55,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��236��6�   $��56000��6�   �� 5   
   $
��Left=�6   $��	66,23,0,0q�6/   $��Destination IP Address��6B   $
��Wrap=�6`   $	��Top=�6t   $��16��6�   $	��119��6�   �� 5   
   $
��Left=�6   $��
317,23,0,0q�6/   $��Destination Port��6C   $
��Wrap=�6[   $	��Top=�6o   $��16��6�   $��85��6�   �� 5   
   -   5         ��Name$ btnGioca��5      $��Gioca!��6#   $
��Left=�64   $��317,147,0,0q�6O   $	��Top=�6d   $��63��6|   $	��102��6�    J� RenderTransformOrigin$ 0.738,0.556&�6�   �� 5   
   -   5        	 ��Name$	 lstSCF��5      $��66,116,508,29q�6"   �� 5   
   -   5        
 ��Name$
 lblRisultato��5      $��
RISULTATO:��6&   $
��Left=�6;   $��
558,81,0,0q�6V   $	��Top=�6j   $	��232��6�   $��26��6�   �� 5   
   $��HAI GIOCATO:��6   $
��Left=�6'   $��558,248,0,0q�6B   $	��Top=�6W   $	��238��6o   $��26��6{   �� 5   
   -   5         ��Name$ imgSCF��5      $
��Left=�6    $	��123��6;   $��558,283,0,0q�6H   $	��Top=�6]   $	��219��6u   �� 5   
   -	   5        $ imgWLD��5      $
��Left=�6    $	��100��6;   $��558,147,0,0q�6H   $	��Top=�6]   $	��219��6u   �� 5   
   -
   5        $
 lblGiocataAvversario��5      $��L'Avversario ha tirato:��6.   $
��Left=�6P   $��558,112,0,0q�6k   $	��Top=�6�   $ 0.354,-0.189&�6�   $	��238��6�   �� 5   
   -   5        $ 
btnRigioca��5      $��Rigioca!��6%   $
��Left=�68   $��317,248,0,0q�6S   $	��Top=�6h   $��75��6�   $	��102��6�   6�   �� 5   
   -   5        $
 lblRound��5      $��Round:��6"   $
��Left=�63   $��
633,13,0,0q�6N   $	��Top=�6b   $	��157��6z   5      5      