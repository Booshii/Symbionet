<template>
  <div class="iframe-container">
    <iframe
      id="marketplace"
      :src="iframeUrl"
      frameborder="0"
      allowfullscreen
      @load="sendUserData"
      >
    </iframe>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/store/authStore';

  const iframeUrl = import.meta.env.VITE_MARKETPLACE_URL;

  function sendUserData() {
    const iframe = document.getElementById('marketplace'); 
    if (!iframe) {
      console.error('Iframe nicht gefunden!');
      return;
    }

    const authStore = useAuthStore();
    const user = authStore.user; 
    const token = localStorage.getItem('access_token');


    if (!token) {
      console.error('Kein Token vorhanden');
      return;
    }

    iframe.contentWindow.postMessage({
      type: 'user-info',
      payload: { 
        userId: user.id, 
        username: user.username,
        email: user.email,
        token: token }
    }, import.meta.env.VITE_MARKETPLACE_URL);
  }
</script>


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


