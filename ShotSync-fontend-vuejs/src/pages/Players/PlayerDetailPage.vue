<template>
  <q-page class="row q-pt-lg q-mt-lg justify-evenly">
    <div class="col justify-center items-center">
      <div class="row justify-center items-center" v-if="loading" style="height: 60vh">
        <q-spinner-cube color="orange" size="5.5em" />
      </div>
    </div>
    <div class="column col-12" v-if="!loading">
      <div class="row justify-center" style="aspect-ratio: 1">
        <div class="column col-6 bg-no-1 img-part q-pa-md" style="aspect-ratio: 1">
          <img :src="player.portrait || ''" class="contain fit" />
        </div>
        <div class="column col-4 bg-no-3 inf-part q-pt-xl q-pl-xl q-mx-lg">
          <div
            class="text-h2 text-secondary text-weight-medium text-italic font-roboto-condensed text-uppercase"
          >
            {{ player.name }}
          </div>
          <div
            class="text-h4 text-gray text-weight-medium text-italic font-roboto-condensed text-uppercase"
          >
            {{ player.nation }}
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import { ref, onMounted } from 'vue'
import type { Player } from '../../components/models'
import api from 'src/services/api'

const loading = ref(true)
const route = useRoute()
const playerName = route.params.playerName
const playerNameStr = Array.isArray(playerName) ? playerName[0] : playerName || 'error'

const player = ref<Player>({
  name: '',
  nation: '',
  portrait: '',
  point: '',
})

const fetchPlayers = async () => {
  try {
    const response = await api.get(`players/${playerNameStr}`)

    const data = await response.data[0]
    player.value = data
    if (player.value && !player.value.portrait) {
      player.value.portrait =
        'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp'
    }
  } catch (error) {
    console.error('Error fetching players:', error)
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  await fetchPlayers()
  console.log(player)
})
</script>

<style lang="sass" scoped>
.inf-part
    height: 40vw
    max-height: 650px
    border-radius: 0 20px 20px 0
.img-part
    height: 40vw
    max-height: 650px
    max-width: 650px
    border-radius: 20px 0 0 20px
</style>
