import { useRouter } from 'vue-router';
<template>
  <q-intersection transition="scale" class="player-card-item q-ma-xs">
    <q-card class="my-card  bg-no-3 cursor-pointer" @click="onPlayerClick" flat >
      <q-card-section horizontal >
        <q-card-section class="q-pa-sm">
          <q-img
            class="col"
            :src="playerPortrait||defaultPortrait"
            style="height: 100px; width: 100px;"
          />
        </q-card-section>
        <q-card-section vertical class="column justify-center items-left q-pa-none">
          <div class="text-h6 text-primary text-uppercase text-italic">{{playerName}}</div>
          <div class="text-h7 text-primary text-uppercase text-italic">{{playerNation}}</div>
        </q-card-section>
      </q-card-section>
    </q-card>
  </q-intersection>
</template>
<script lang='ts' setup>
import { useRouter } from "vue-router";
const router = useRouter()
export interface PlayerCardProps{
    playerName: string
    playerNation: string
    playerPortrait: string
}

const { playerName = '', playerNation = '', playerPortrait = 'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp' } = defineProps<PlayerCardProps>();

const defaultPortrait = 'https://storage.googleapis.com/wnt-cm-public/media/players/generic-profile_mini_dcryfs.webp';

const onPlayerClick = async () =>{
  await router.push(`players/${playerName}`)
}
</script>
<style lang="sass" scoped>
.player-card-item
  width: 30%
</style>
