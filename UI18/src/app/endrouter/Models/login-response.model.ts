export class LoginResponse {
    public email!: string;
    public fullName!: string;
    public token!: string;
    public expiresIn!: number;
    public userId!: string;
    public name!: string;
    public statusCode: any;

}


export class LoginApiResponse {
  data!: LoginResponse;
  statusCode!: number;
}
