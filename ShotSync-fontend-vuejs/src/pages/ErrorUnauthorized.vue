<template>
  <div class="flex flex-center q-pa-md" style="min-height: 100vh">
    <div class="text-center">
      <div class="text-h4 q-mb-md">401 - Unauthorized</div>
      <div class="text-subtitle1 q-mb-lg">
        You don't have permission to access this page.
      </div>
      <q-btn
        color="primary"
        label="Go to Login"
        @click="$router.push('/login')"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
// Unauthorized page
</script>
