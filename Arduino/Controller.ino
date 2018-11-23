const int space = 7;

void setup() {
  Serial.begin(9600);

  pinMode(space, INPUT);
  digitalWrite(space, HIGH);
}

void loop() {
  if(digitalRead(space) == LOW)
  {
    Serial.println(1);
    Serial.flush();
    delay(20);
  }
}
