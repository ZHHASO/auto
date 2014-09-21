#include<reg52.h> //����ͷ�ļ���һ���������Ҫ�Ķ���ͷ�ļ��������⹦�ܼĴ����Ķ���
#include<intrins.h>

sbit RS = P3^5;   //����˿� 
sbit RW = P3^6;
sbit EN = P3^7;
sbit CU = P2^6;
sbit MP = P2^7;
sbit HB = P3^3;
sbit HL = P3^4;

#define RS_CLR RS=0 
#define RS_SET RS=1

#define RW_CLR RW=0 
#define RW_SET RW=1 

#define EN_CLR EN=0
#define EN_SET EN=1

#define DataPort P1
#define MotorPort P2

/*------------------------------------------------
 uS��ʱ����������������� unsigned char t���޷���ֵ
 unsigned char �Ƕ����޷����ַ���������ֵ�ķ�Χ��
 0~255 ����ʹ�þ���12M����ȷ��ʱ��ʹ�û��,������ʱ
 �������� T=tx2+5 uS 
------------------------------------------------*/
void DelayUs2x(unsigned char t)
{   
 while(--t);
}
/*------------------------------------------------
 mS��ʱ����������������� unsigned char t���޷���ֵ
 unsigned char �Ƕ����޷����ַ���������ֵ�ķ�Χ��
 0~255 ����ʹ�þ���12M����ȷ��ʱ��ʹ�û��
------------------------------------------------*/
void DelayMs(unsigned int t)
{
     
 while(t--)
 {
     //������ʱ1mS
     DelayUs2x(245);
	 DelayUs2x(245);
 }
}
/*------------------------------------------------
              ��æ����
------------------------------------------------*/
 bit LCD_Check_Busy(void) 
 { 
 DataPort= 0xFF; 
 RS_CLR; 
 RW_SET; 
 EN_CLR; 
 _nop_(); 
 EN_SET;
 return (bit)(DataPort & 0x80);
 }
/*------------------------------------------------
              д�������
------------------------------------------------*/
 void LCD_Write_Com(unsigned char com) 
 {  
 while(LCD_Check_Busy()); //æ��ȴ�
 RS_CLR; 
 RW_CLR; 
 EN_SET; 
 DataPort= com; 
 _nop_(); 
 EN_CLR;
 }
/*------------------------------------------------
              д�����ݺ���
------------------------------------------------*/
 void LCD_Write_Data(unsigned char Data) 
 { 
 while(LCD_Check_Busy()); //æ��ȴ�
 RS_SET; 
 RW_CLR; 
 EN_SET; 
 DataPort= Data; 
 _nop_();
 EN_CLR;
 }

/*------------------------------------------------
                ��������
------------------------------------------------*/
 void LCD_Clear(void) 
 { 
 LCD_Write_Com(0x01); 
 DelayMs(5);
 }
/*------------------------------------------------
              д���ַ�������
------------------------------------------------*/
 void LCD_Write_String(unsigned char x,unsigned char y,unsigned char *s) 
 {     
 if (y == 0) 
 	{     
	 LCD_Write_Com(0x80 + x);     //��ʾ��һ��
 	}
 else 
 	{      
 	LCD_Write_Com(0xC0 + x);      //��ʾ�ڶ���
 	}        
 while (*s) 
 	{     
 LCD_Write_Data( *s);     
 s ++;     
 	}
 }
/*------------------------------------------------
              д���ַ�����
------------------------------------------------*/
 void LCD_Write_Char(unsigned char x,unsigned char y,unsigned char Data) 
 {     
 if (y == 0) 
 	{     
 	LCD_Write_Com(0x80 + x);     
 	}    
 else 
 	{     
 	LCD_Write_Com(0xC0 + x);     
 	}        
 LCD_Write_Data( Data);  
 }
/*------------------------------------------------
              д���ַ�����
------------------------------------------------*/
void LCD_WriteNumber(unsigned char x,unsigned char y,unsigned int i)
{
	unsigned char Data[]={'0','1','2','3','4','5','6','7','8','9'};
	LCD_Write_Char(x,y,Data[i/10000]);
	LCD_Write_Char(x+1,y,Data[i%10000/1000]);	
  	LCD_Write_Char(x+2,y,Data[i%1000/100]);
   	LCD_Write_Char(x+3,y,Data[i%100/10]);
	LCD_Write_Char(x+4,y,Data[i%10]);
}

/*------------------------------------------------
              ��ʼ������
------------------------------------------------*/
 void LCD_Init(void) 
 {
   LCD_Write_Com(0x38);    /*��ʾģʽ����*/ 
   DelayMs(5); 
   LCD_Write_Com(0x38); 
   DelayMs(5); 
   LCD_Write_Com(0x38); 
   DelayMs(5); 
   LCD_Write_Com(0x38);  
   LCD_Write_Com(0x08);    /*��ʾ�ر�*/ 
   LCD_Write_Com(0x01);    /*��ʾ����*/ 
   LCD_Write_Com(0x06);    /*��ʾ����ƶ�����*/ 
   DelayMs(5); 
   LCD_Write_Com(0x0C);    /*��ʾ�����������*/
   }

/*------------------------------------------------
              ���ڷ���
------------------------------------------------*/
void SendOneByte(unsigned char c)
{
    SBUF = c;
    while(!TI);
    TI = 0;
}
/*------------------------------------------------
              ���ڳ�ʼ��
------------------------------------------------*/
void InitUART(void)
{
    TMOD = 0x20;
    SCON = 0x50;
    TH1 = 0xFD;
    TL1 = TH1;
    PCON = 0x00;
    EA = 1;
    ES = 1;
    TR1 = 1;
}
/*------------------------------------------------
              д�������
------------------------------------------------*/
void  Motor_Select(int mt)
{
	if(mt < 64)
	{
		MotorPort = mt;
		MP = 1;
		CU = 1;
	}
}
 
  
/*------------------------------------------------
                    ������
------------------------------------------------*/ 
void main(void) 
{ 
DataPort = 0x00;
HB = 0;
HL = 0;
InitUART(); 
//LCD_Init();
//LCD_Clear();//���� 
// LCD_Write_String(0,0,"Auto Parts Box");
// LCD_Write_String(0,1,"ZHHASO develop");
  while(1)
  {
    if(!CU)
    {
	DelayMs(400);
		if(!CU)
		{
			HB = 0;
			HL = 0;
		}
	
   }
  }
}

/*------------------------------------------------
              �����ж�
------------------------------------------------*/ 
 
 void UARTInterrupt(void) interrupt 4
{
	unsigned char Data1;
	unsigned char Data2;
	unsigned char Data3;
	unsigned char Data4;
	unsigned int t;
	bit OK = 0;
    if(RI)
    {
	    RI = 0;	
		if(SBUF == 'K')
		{
			SendOneByte('K');
		//	LCD_Clear();
		//	LCD_Write_String(0,0,"Connect success");
			OK = 1;
		}
		Data1 = SBUF;
		t = 5000;
		while(!RI&&!OK&&t--);
		RI = 0;
		Data2 = SBUF;
		t = 5000;
		while(!RI&&!OK&&t--);
		RI = 0;
		Data3 = SBUF;
		t = 5000;
		while(!RI&&!OK&&t--);
		RI = 0;
		Data4 = SBUF;
		if(Data1 == 0x7f)
		{
			if(MP)
			{
				SendOneByte(0xf3);
			//	LCD_Clear();
			//	LCD_Write_String(0,0,"No Motor Power");
			}
			else
			{
		    SendOneByte(0xf4);
			if(Data2 == 0x80)
				{
				//	LCD_Clear();
				//	LCD_Write_String(0,0,"Eject Location:");
				//	LCD_WriteNumber(0,1,(Data3 * 256) + Data4);
					Motor_Select((Data3 * 256) + Data4);
					HL = 0;
					HB = 0;
					DelayMs(200);
					HL = 1;
					HB = 0;
				}
			else if(Data2 == 0x81)
				{
				//	LCD_Clear();
				//	LCD_Write_String(0,0,"Back Location:");
				//	LCD_WriteNumber(0,1,(Data3 * 256) + Data4);
					Motor_Select((Data3 * 256) + Data4);
					HL = 0;
					HB = 0;
					DelayMs(200);
					HB = 1;
					HL = 0;
				}
			}
		} 
    }
    else
        TI = 0;
}

