export class LoginRequest {
  constructor(
    public email: string,
    public password: string,
    // public recaptchaToken: string 
  ) {}
}
