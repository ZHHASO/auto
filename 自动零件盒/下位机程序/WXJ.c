#include<reg52.h> //包含头文件，一般情况不需要改动，头文件包含特殊功能寄存器的定义
#include<intrins.h>

sbit RS = P3^5;   //定义端口 
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
 uS延时函数，含有输入参数 unsigned char t，无返回值
 unsigned char 是定义无符号字符变量，其值的范围是
 0~255 这里使用晶振12M，精确延时请使用汇编,大致延时
 长度如下 T=tx2+5 uS 
------------------------------------------------*/
void DelayUs2x(unsigned char t)
{   
 while(--t);
}
/*------------------------------------------------
 mS延时函数，含有输入参数 unsigned char t，无返回值
 unsigned char 是定义无符号字符变量，其值的范围是
 0~255 这里使用晶振12M，精确延时请使用汇编
------------------------------------------------*/
void DelayMs(unsigned int t)
{
     
 while(t--)
 {
     //大致延时1mS
     DelayUs2x(245);
	 DelayUs2x(245);
 }
}
/*------------------------------------------------
              判忙函数
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
              写入命令函数
------------------------------------------------*/
 void LCD_Write_Com(unsigned char com) 
 {  
 while(LCD_Check_Busy()); //忙则等待
 RS_CLR; 
 RW_CLR; 
 EN_SET; 
 DataPort= com; 
 _nop_(); 
 EN_CLR;
 }
/*------------------------------------------------
              写入数据函数
------------------------------------------------*/
 void LCD_Write_Data(unsigned char Data) 
 { 
 while(LCD_Check_Busy()); //忙则等待
 RS_SET; 
 RW_CLR; 
 EN_SET; 
 DataPort= Data; 
 _nop_();
 EN_CLR;
 }

/*------------------------------------------------
                清屏函数
------------------------------------------------*/
 void LCD_Clear(void) 
 { 
 LCD_Write_Com(0x01); 
 DelayMs(5);
 }
/*------------------------------------------------
              写入字符串函数
------------------------------------------------*/
 void LCD_Write_String(unsigned char x,unsigned char y,unsigned char *s) 
 {     
 if (y == 0) 
 	{     
	 LCD_Write_Com(0x80 + x);     //表示第一行
 	}
 else 
 	{      
 	LCD_Write_Com(0xC0 + x);      //表示第二行
 	}        
 while (*s) 
 	{     
 LCD_Write_Data( *s);     
 s ++;     
 	}
 }
/*------------------------------------------------
              写入字符函数
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
              写入字符函数
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
              初始化函数
------------------------------------------------*/
 void LCD_Init(void) 
 {
   LCD_Write_Com(0x38);    /*显示模式设置*/ 
   DelayMs(5); 
   LCD_Write_Com(0x38); 
   DelayMs(5); 
   LCD_Write_Com(0x38); 
   DelayMs(5); 
   LCD_Write_Com(0x38);  
   LCD_Write_Com(0x08);    /*显示关闭*/ 
   LCD_Write_Com(0x01);    /*显示清屏*/ 
   LCD_Write_Com(0x06);    /*显示光标移动设置*/ 
   DelayMs(5); 
   LCD_Write_Com(0x0C);    /*显示开及光标设置*/
   }

/*------------------------------------------------
              串口发送
------------------------------------------------*/
void SendOneByte(unsigned char c)
{
    SBUF = c;
    while(!TI);
    TI = 0;
}
/*------------------------------------------------
              串口初始化
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
              写入命令函数
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
                    主函数
------------------------------------------------*/ 
void main(void) 
{ 
DataPort = 0x00;
HB = 0;
HL = 0;
InitUART(); 
//LCD_Init();
//LCD_Clear();//清屏 
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
              串口中断
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

