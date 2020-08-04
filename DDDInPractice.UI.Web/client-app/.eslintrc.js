module.exports = {
  root: true,
  env: {
    node: true
  },
  'extends': [
    'plugin:vue/essential',
    // 'eslint:recommended',
    '@vue/typescript/recommended'
  ],
  parserOptions: {
    ecmaVersion: 2020
  },
  rules: {
    'no-console': 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    '@typescript-eslint/ban-ts-ignore' : 'off',
    'prefer-const' : 'off',
    '@typescript-eslint/no-var-requires': 'off',
    'no-var': 'off',
    '@typescript-eslint/no-use-before-define': 'off',
    '@typescript-eslint/no-empty-function': 'off',
    'prefer-spread': 'off',
    'vue/no-use-v-if-with-v-for': 'off',
  }
}
