<template>
  <q-page class="row justify-center">
    <div class="col"></div>
    <div class="col justify-center">
      <div class="row justify-center items-center" v-if="loading" style="height: 60vh">
        <q-spinner-cube color="orange" size="5.5em" />
      </div>
    </div>
    <div class="col-10 q-py-md column" v-if="!loading">
      <div
        class="event-infor-component w-100 bg-no-2 q-my-sm row"
        style="height: auto; border-radius: 20px; width: 100%"
      >
        <div class="col column justify-center items-center">
          <div
            class="row bg-primary text-white text-h6 justify-center"
            style="height: 40px; width: 30%; border-radius: 0 0 10px 10px"
          >
            PLAYER LIST
          </div>
          <div class="row q-my-lg justify-center" style="width: 95%">
            <PlayerCardComponent
              v-for="(player, index) in players"
              :key="index"
              :playerName="player.name"
              :playerNation="player.nation"
              :playerPortrait="player.portrait || defaultPortrait"
            />
          </div>
        </div>
      </div>
    </div>
    <div class="col"></div>
  </q-page>
</template>

<script setup lang="ts">
//import { useRoute } from 'vue-router'

// Sử dụng useRoute để lấy thông tin về route hiện tại
//const route = useRoute()

// Truy xuất eventSlug từ params của route
//const eventSlug = route.params.eventSlug

import PlayerCardComponent from 'components/PlayerCardComponent.vue'
import { ref, onMounted } from 'vue'
import type { Player } from '../../components/models'
import api from 'src/services/api'

const loading = ref(true)

const defaultPortrait =
  'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp'

const players = ref<Player[]>([])

const fetchPlayers = async () => {
  try {
    const response = await api.get('/players')

    const data = response.data
    players.value = data
  } catch (error) {
    console.error('Error fetching players:', error)
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  await fetchPlayers()
})
</script>
