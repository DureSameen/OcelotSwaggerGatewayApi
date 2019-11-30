import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators'
import { AuthService } from '../../core/authentication/auth.service';
import { ShoppingService } from '../shopping.service';

@Component({
  selector: 'top-secret-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  users=null;
  busy: boolean;

    constructor(private authService: AuthService, private shoppingService: ShoppingService, private spinner: NgxSpinnerService) {
  }
    order() { 
        this.shoppingService.doShopping(this.authService.authorizationHeaderValue, this.authService.userId, this.authService.tenantId);
    }   
  ngOnInit() {    
   
  }
}
