<template>
  <div
    class="player-component bg-no-2 q-my-sm row cursor-pointer" @click="onPlayerClick"
    style="height: 100px; border-radius: 20px;"
  >
    <div class="col text-left self-center q-pl-md  text-h4 text-bold">
      #{{ playerNumber }}
    </div>

    <div class="col-9 text-left self-center">
      <div class="row">
        <div class="col">
          <img :src="playerPortrait||defaultPortrait" alt="My Image" width="90" height="80" />
        </div>
        <div class="col-10 text-left self-center">
          <div class="player-name  text-h4 text-weight-medium text-italic font-roboto-condensed text-uppercase">
            {{ playerName }}
          </div>
          <div class="player-nation  text-h7 text-uppercase">{{ playerNation }}</div>
        </div>
      </div>
    </div>
    <div class="col-2 text-right self-center q-pr-md  text-h5 text-italic">{{ playerPoint }}</div>
  </div>
</template>
<script setup lang="ts">
import { useRouter } from 'vue-router'
const router = useRouter()


export interface PlayerComponentProps {
  playerNumber: number
  playerName: string
  playerNation: string
  playerPoint: string
  playerPortrait?: string
}

const { playerNumber = 0, playerName = 'John Doe', playerNation = 'America', playerPoint = '100,000 USD', playerPortrait } = defineProps<PlayerComponentProps>();

const defaultPortrait = 'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp';

// Define click handler

const onPlayerClick = async () =>{
  await router.push(`players/${playerName}`)
}
</script>
<style lang="scss">

</style>
