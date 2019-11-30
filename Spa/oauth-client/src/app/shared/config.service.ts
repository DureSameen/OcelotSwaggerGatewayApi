import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {    

    constructor() {}

    get authApiURI() {
        return 'http://localhost:5000/api';
    }    
     
    get resourceApiURI() {
        return 'http://localhost:5050/api';
    }

    get OcelotApiURI() {
        return 'http://localhost:59714/api';
    }
    get paymentApiUri() {
        return 'https://localhost:44378/api';
    }
}