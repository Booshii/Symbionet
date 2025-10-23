<template>
    <div class="iframe-container">
      <iframe
        src="https://dash-gruenesk.n1.dezem.io/home2/dashboard/#!dashboard/247299&mode=kiosk"
        frameborder="0"
        allowfullscreen>
      </iframe>
    </div>
  </template>
  
  <style scoped>
    .iframe-container {
        position: relative;
        width: 100%;
        padding-top: 56.25%; /* Verhältnis 16:9 */
    }
    
    .iframe-container iframe {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        border: none;
    }
  </style>