export default class UserStats {
    constructor(public name: string,
        public surname: string,
        public phoneNumber: string,
        public contractTerm: number,
        public balance: number,
        public smsBalance: number,
        public smsLimit: number,
        public internetBalance: number,
        public internetLimit: number,
        public voiceBalance: number,
        public voiceLimit: number
    ) { }
}