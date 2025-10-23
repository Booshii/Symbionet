<template>
    <div class="container-account-view">

         <iframe
            id="materialdatabase"
            :src="iframeUrl"
            frameborder="0"
            allowfullscreen
        ></iframe>
    </div>
</template>

<script setup>
  const iframeUrl = import.meta.env.VITE_LCIA_DATABASE_URL;

</script>

<style scoped>
.container-account-view {
  width: 100%;
  height: 100%;
  padding: 0;
  margin: 0;
}

iframe#materialdatabase {
  width: 100%;
  height: 90vh; /* Nimmt 90% der sichtbaren Höhe ein */
  border: none;
}
</style>
