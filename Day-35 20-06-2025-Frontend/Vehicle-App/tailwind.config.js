const plugin = require('tailwindcss/plugin')

module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      backgroundImage: {
        'blobs': "url('/assets/blob-bg.svg')",
        'warm-gradient': 'linear-gradient(to right, #facc15, #f87171)', // smooth yellow to red
        'sunset': 'linear-gradient(135deg, #fde68a 0%, #f87171 100%)' // more gentle blend
      },
      colors: {
        primary: '#f59e0b',       // amber-500 (strong yellow-orange)
        secondary: '#ef4444',     // red-500 (vibrant red)
        accent: '#fcd34d',        // yellow-300 for light accents
        blurOverlay: 'rgba(255, 245, 204, 0.3)', // subtle warm overlay
        textHeading: '#b91c1c',   // deep red for bold text
        textSubtle: '#78350f',    // warm brown for subtle UI
      },
      backdropBlur: {
        xs: '2px',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
}
