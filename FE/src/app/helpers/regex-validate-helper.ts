export class RegexHelper {
    static emailRegexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
    static phoneRegex = new RegExp(/^\d{10}$/);

    static isValidEmail(email: string): boolean {
        if (email == null || email == '') {
            return false;
        }
        return this.emailRegexp.test(email);
    }

    static isValidPhoneNumber(phone: string): boolean {
        if (phone == null || phone == '') {
            return false;
        }
        return this.phoneRegex.test(phone);
    }
} 