import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule }   from '../shared/shared.module';
import { IndexComponent } from './index/index.component';

import { ShoppingService }  from '../shopping/shopping.service';

import { ShoppingRoutingModule } from './shopping.routing-module';

@NgModule({
  declarations: [IndexComponent],
    providers: [ShoppingService],
  imports: [
    CommonModule,  
    ShoppingRoutingModule,
    SharedModule
  ]
})
export class ShoppingModule { }
