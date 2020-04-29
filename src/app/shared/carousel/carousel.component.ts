import { Component, OnInit } from '@angular/core';
import { carouselDatas } from 'src/app/shared/models/mock-carousel';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.css']
})
export class CarouselComponent implements OnInit {
  carouselItems;

  constructor() {
    this.carouselItems = carouselDatas;
  }

  ngOnInit() {
  }

  checkLogs(): void {
    console.log('Dispatched');
  }
}
