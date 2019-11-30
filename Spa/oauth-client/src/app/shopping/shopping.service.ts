import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BaseService } from "../shared/base.service";
import { ConfigService } from '../shared/config.service';

@Injectable({
  providedIn: 'root'
})

export class ShoppingService extends BaseService {

  constructor(private http: HttpClient, private configService: ConfigService) {    
    super();      
  }

  doShopping(token: string, userId:string, tenantId:string) {   
    
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };
        var order = {
        "tenantId":   tenantId ,
            "customerId":  userId  ,
                "orderDetails": [
                    {
                        "productId": 5,
                        "productUId": "b0dfd6a8-4242-4a3a-9292-fd6f360492df",
                        "productName": "Iron",
                        "price": 50.00,
                        "quantity": 1
                    },
                    {
                        "productId": 9,
                        "productUId": "7c128284-ba11-4970-b18b-37a3d00ab4da",
                        "productName": "Window Cleaner",
                        "price": 25.44,
                        "quantity": 1
                    }
                ],
                    "cardDetails": {
            "number": "4242424242424242",
                "expYear": 2020,
                    "expMonth": 11,
                        "cvc": "123"
        }
    }
      
    return this.http.post(this.configService.paymentApiUri + '/order',  order , httpOptions).pipe(catchError(this.handleError)).subscribe();;
  }
}
