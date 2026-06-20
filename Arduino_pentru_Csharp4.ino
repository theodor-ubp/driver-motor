#include <LiquidCrystal_I2C.h>
LiquidCrystal_I2C lcd(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);
#define TRIGGER_PIN 4
#define ECHO_PIN 7
#define Motor 5
#define RESET       2
#define INAINTE     1
#define STOP        0
int ok = 1;
#define CONVERSIE_TIMP_DIST 58
#define TIMP_ESANTIONARE 5
#define MEDIERE_ESANTION 25
#define TIMP_CITIRE 50

void setup()
{
  digitalWrite(8, HIGH);
  pinMode(8, OUTPUT);
  pinMode(Motor, OUTPUT);
  pinMode(TRIGGER_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);
  lcd.begin(16, 2);
  lcd.backlight();
  lcd.setCursor(0, 0);
  lcd.print("Verificare batai");
  lcd.setCursor(0, 1);
  lcd.print("frontale  v3");
  delay(500);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("FIMM 541B");
  lcd.setCursor(0, 1);
  lcd.print("Absolvent:");
  delay(500);
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Theodor-Andrei");
  lcd.setCursor(0, 1);
  lcd.print("Popescu");
  delay(500);
  Serial.begin(9600);
  digitalWrite(TRIGGER_PIN, LOW);
  delayMicroseconds(500);
}
void loop()
{
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Apasati START");
  lcd.setCursor(0, 1);
  lcd.print("Abatere:");
  lcd.setCursor(14, 1);
  lcd.print("mm");
  AnimatieText("START", 8, 0, 2);
//  while (ok == 1) {
    if (Serial.available())
    {
      int Comanda = Serial.read();
      int Viteza = Comanda % 10;
      int Directie;// = Comanda / 10;
      int PWM_Viteza = map(Viteza, 1, 9, 60, 255);
      lcd.setCursor(0, 0);
      lcd.print("Valoare PWM:");
      lcd.setCursor(12, 0);
      lcd.print(PWM_Viteza);
      if (PWM_Viteza < 100)
      {
        lcd.print("  ");
      }
      if (PWM_Viteza < 10)
      {
        lcd.print(" ");
      }
      if (PWM_Viteza == 36)
      {
        lcd.setCursor(12, 0);
        lcd.print(" 0");
      }
      switch (Directie)
      {
        case STOP:        analogWrite(Motor, 0);
          break;
        case INAINTE:     analogWrite(Motor, PWM_Viteza);
          break;
        case RESET:       digitalWrite(8, LOW);
          break;
      }
    }
    citire_senzori();
// }
}


int citire_senzori() {
  delay(TIMP_CITIRE);
  long distanta = masoara();
  Serial.println(distanta - 44);
  lcd.setCursor(12, 1);
  lcd.print(distanta - 44);
  if (distanta - 44 < 0)
  {
    lcd.setCursor(12, 1);
    lcd.print(" ");
  }
  if (distanta < 10)
  {
    lcd.print(" ");
  }
  if (Serial.available())
  {
    lcd.clear();
    lcd.setCursor(0, 1);
    lcd.print("Abatere:");
    lcd.setCursor(14, 1);
    lcd.print("mm");
  }
}

void AnimatieText(char *msg, int col, int row, int repeat)
{
  char temp[] = "        ";

  lcd.setCursor(col, row);
  lcd.print(msg);
  for (int i = 0; i < repeat; i++) {
    lcd.setCursor(col, row);
    lcd.print(temp);
    delay(500);
    lcd.setCursor(col, row);
    lcd.print(msg);
    delay(500);
  }
}

long masoara()
{
  long masoaraSuma = 0;
  for (int k = 0; k < MEDIERE_ESANTION; k++)
  {
    delay(TIMP_ESANTIONARE);
    masoaraSuma += unesantion();
  }
  return masoaraSuma / MEDIERE_ESANTION;
}

long unesantion()
{
  long durata = 0; //Se setează referința
  digitalWrite(TRIGGER_PIN, HIGH);
  //Se transmite unda ultrasonică
  delayMicroseconds(11);
  //Se așteaptă 11µs
  digitalWrite(TRIGGER_PIN, LOW);
  //Se oprește transmisia
  durata = pulseIn(ECHO_PIN, HIGH);
  //Se așteaptă revenirea
  return (long) (((float) durata / CONVERSIE_TIMP_DIST) * 10.0);
  //Se returnează valoarea în mm
}
