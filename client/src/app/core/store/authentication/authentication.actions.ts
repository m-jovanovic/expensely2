export class SignIn {
    static readonly type = '[Authentication] Sign In';

    constructor(public email: string, public password: string) {}
}

export class SignOut {
    static readonly type = '[Authentication] Sign Out';
}