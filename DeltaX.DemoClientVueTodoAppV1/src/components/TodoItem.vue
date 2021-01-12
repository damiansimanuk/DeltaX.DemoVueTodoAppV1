<template>
  <li
    class="flex row items-center justify-between w-full py-0 px-0 my-1 rounded border bg-gray-100 text-gray-600"
  > 
    <div class="flex flex-grow column my-0 px-2 py-0" >
      <input
        class="mr-2"
        type="checkbox"
        :checked="editTodo.completed"
        @input="onUpdate(!editTodo.completed)"
      > 
      <span 
        v-show = "editTodo.edit == false" 
        class="w-full h-10 py-1 px-2 text-left" 
        :class="{'line-through': editTodo.completed}" 
        @dblclick = "editTodo.edit = true"
        style="cursor:pointer">
        {{ editTodo.description }}
      </span>
      <input
        class="shadow appearance-none border rounded h-10 w-full my-0 py-1 px-2 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        v-show = "editTodo.edit == true"
        type="text"
        v-model = "editTodo.description" 
        v-on:blur = "onCancelUpdate()"
        @keyup.esc = "onCancelUpdate()"
        @keyup.enter = "onUpdate()" />
    </div>  
    <div class="row-reverse flex-none">
      <button  class="bg-white hover:bg-blue-100 mx-0 text-gray-800 font-semibold h-10 py-2 px-2 ripple rounded flat" 
        @click="onRemove()">
        <svg class="w-5 h-5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
        </svg>
      </button>
    </div> 
  </li>
</template>

<script lang="ts"> 
import { PropType, ref, watch, defineComponent } from 'vue' 
import { TodoDto } from "@/dtos/TodoDto";

type TodoEdit = TodoDto & {
  edit?: boolean; 
}

export default defineComponent({
  props: {
      todo: {
          type: Object as PropType<TodoDto>,
          required: true
      }
  },

  emits:["update", "remove"],  
    
  setup (prop, ctx) {
    const editTodo = ref({ ...prop.todo, edit:false, newDescription:prop.todo.description } as TodoEdit)

    watch(prop, () => {
      editTodo.value.description = prop.todo.description;
      editTodo.value.completed = prop.todo.completed;
    })  

    const onUpdate = (completed?: boolean) => {  
      console.log("onUpdate", editTodo.value)
      const done: boolean = completed != null ? completed : editTodo.value.completed
      ctx.emit("update", editTodo.value.id, editTodo.value.description, done )
      editTodo.value.edit = false 
    }

    const onCancelUpdate = () => {   
      console.log("onCancelUpdate", editTodo.value)
      editTodo.value.description = prop.todo.description;
      editTodo.value.completed = prop.todo.completed;
      editTodo.value.edit = false
    }

    const onRemove = () => {   
      console.log("onRemove", editTodo.value)
      ctx.emit("remove", editTodo.value.id)
    }

    return{
      editTodo, 
      onUpdate,
      onCancelUpdate,
      onRemove
    }
  } 
}) 
</script>