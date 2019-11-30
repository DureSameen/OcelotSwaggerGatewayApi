import { of } from 'rxjs';


export class MockShoppingService { 
    order(token: string) {    
        return of([1,2,3,4,5]); 
      }   
}