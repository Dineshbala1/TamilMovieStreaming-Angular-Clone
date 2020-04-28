import { Component, OnInit } from '@angular/core';
import {headerMenuItems} from '../models/mock-header';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit {
  isNavbarCollapsed = true;
  navigationMenuItems;

  constructor() {
    this.navigationMenuItems = headerMenuItems;
  }

  ngOnInit() {
  }

}
