import {
  Component,
  OnInit,
  OnDestroy,
  AfterViewInit,
  ElementRef,
  Input
} from '@angular/core';

declare const videojs: any;

@Component({
  selector: 'sxm-video',
  templateUrl: './video.component.html',
})

export class VideoComponent implements OnInit, AfterViewInit {

  // reference to the element itself, we use this to access events and methods
  private _elementRef: ElementRef

  seekbarTracker: any = {
    duration: 0,
    time: 0,
    seekPercent: 0,
    hasDVR: false,
  };

  seekTime: number;
  // // video asset url
  // @Input() url: any;

  // declare player var
  private player: any;

  // constructor initializes our declared vars
  constructor(elementRef: ElementRef) {
    // this.url = false;
    this.player = false;
  }

  ngOnInit() { }

  // use ngAfterViewInit to make sure we initialize the videojs element
  // after the component template itself has been rendered

  ngAfterViewInit() {
    const self = this;
    this.player = videojs(document.getElementById('sxmvideo'));
    console.log(this.player);
    this.player.muted(true);
    this.player.on('timeupdate', () => {
      let hasDVR = false;
      let duration = Math.floor(this.getDuration(this.player) * 1000);
      let time;
      let seekPercent;
      // this.player.controls(true);
      console.log(this.player.currentTime(), this.getDuration(this.player));

      // if(duration) {
      //   this.seekbarTracker.duration = duration;

      //   // constrain time
      //   time = Math.floor(Math.max(0, Math.min(duration, this.player.currentTime() * 1000)));
      //   this.seekbarTracker.time = time; 
      //   seekPercent = time / duration;
      //   if(seekPercent !== this.seekbarTracker.seekPercent) {
      //     this.onSeekPercentChange(this.player, seekPercent, duration);
      //   }
      //   this.seekbarTracker.seekPercent = seekPercent;

      //   // duration is not Infinity, so if isLive() returns true, then player should have DVR.
      //   hasDVR = this.isLive();
      // }

      // this.seekbarTracker.hasDVR = hasDVR;
    });
  }

  getDuration(player) {
    var seekable = player.seekable();
    return seekable && seekable.length ? seekable.end(0) - seekable.start(0) : 0;
  }

  onSeekPercentChange(player, seekPercent, duration) {
    var seekable = player.seekable();

    if (seekable && seekable.length) {
      // constrain currentTime
      player.currentTime(Math.max(0, Math.min(seekable.end(0), seekable.start(0) + (seekPercent * duration))));
    }
  }

  isLive() {
    if (!isFinite(this.player.duration())) {
      return true;
    }

    var acceptableDelay = 30;
    var seekable = this.player.seekable();
    return seekable && seekable.length && seekable.end(0) - this.player.currentTime() < acceptableDelay;
  }

  ngOnDestroy() {

    // this.player.dispose();
  }

  seek(n) {
    this.player.currentTime(this.seekTime || 1266);
  }
  play(n) {

    if (this.player.paused()) {
      this.player.play();
    }
    else {
      this.player.pause();
    }
  }

  create() {
    //     this.player.src({
    //   src: 'https://oocache-live-delivery-ooyala.akamaized.net/out/u/d8npqvovi8we5/110326/U3cWNvZjE6xpWi6dq7FE2Q8B362hEbfl/en/cc984f46656c4ecc889711165c08b378.m3u8'
    // });

    this.player.src({
      src: 'https://cdn3.chromecast.video/hls/r42lrth5pxczrmasbrqc27zozc6j7pzggwuudwrtj,7h73prqmgkqmu54pt2q,va73prqmgku3uuf6teq,plpzprqmgk6fqjmp2dq,.urlset/master.m3u8?hash=1234'
    
    });
    this.player.play();
    // this.player.controlBar.currentTimeDisplay();  
  }

  destroy() {

  }

}