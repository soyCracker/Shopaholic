import { createApp } from 'vue'

let app = createApp({
    data() {
        return {
            message: 5
        };
    },
    methods: {
        AddOne() {
            let self = this;
            console.log("what!");
            self.message++;
            commentApp.commentMsg = self.message;
        }
    }
}).mount('#app');

let commentApp = createApp({
    data() {
        return {
            commentMsg: 3
        };
    }
}).mount('#CommentApp')