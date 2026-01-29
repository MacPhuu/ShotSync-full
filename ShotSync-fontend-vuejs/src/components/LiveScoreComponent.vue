<template>
  <q-intersection transition="scale" class="player-card-item q-ma-xs" v-if="isStart">
    <q-card class="my-card bg-no-3" flat>
      <q-card-section horizontal>
        <q-card-section
          class="q-pa-md column justify-center text-h5 text-weight-bold"
          style="border-right: 1px solid"
        >
          {{ table }}
        </q-card-section>
        <q-card-section vertical class="column justify-center items-left q-py-sm q-px-md">
          <div class="text-h6 text-primary text-capitalize text-italic">{{ firstPlayerName }}</div>
          <div class="text-h6 text-primary text-capitalize text-italic">{{ secondPlayerName }}</div>
        </q-card-section>
        <q-card-section
          vertical
          class="column justify-center items-end q-py-sm q-px-md"
          style="flex-grow: 1"
        >
          <div
            class="text-h6 text-capitalize text-italic"
            :class="{
              winner: isFinish && secondPoint < firstPoint,
            }"
          >
            {{ firstPoint }}
          </div>
          <div
            class="text-h6 text-capitalize text-italic "
            :class="{
              winner: isFinish && secondPoint > firstPoint,
            }"
          >
            {{ secondPoint }}
          </div>
        </q-card-section>
      </q-card-section>
    </q-card>
  </q-intersection>
</template>
<script lang="ts" setup>
export interface LiveScoreComponentProps {
  table: string
  firstPlayerName: string
  secondPlayerName: string
  firstPoint: number
  secondPoint: number
  isFinish: boolean
  isStart: boolean
}

withDefaults(defineProps<LiveScoreComponentProps>(), {
  table: 'T1',
  firstPlayerName: '',
  secondPlayerName: '',
  firstPoint: 0,
  secondPoint: 0,
  isFinish: false,
  isStart: false,
})
</script>
<style lang="sass" scoped>
.player-card-item
  width: 30%

.winner
  color: #ed0202

.player-point
  color: #000033
</style>
