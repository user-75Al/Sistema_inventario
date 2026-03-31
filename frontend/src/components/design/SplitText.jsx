import { useRef, useEffect, useState } from 'react';
import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { useGSAP } from '@gsap/react';

gsap.registerPlugin(ScrollTrigger, useGSAP);

const SplitText = ({
  text,
  className = '',
  delay = 50,
  duration = 1.25,
  ease = 'power3.out',
  from = { opacity: 0, y: 40 },
  to = { opacity: 1, y: 0 },
  threshold = 0.1,
  textAlign = 'center',
  tag = 'p',
}) => {
  const ref = useRef(null);
  const [fontsLoaded, setFontsLoaded] = useState(false);

  useEffect(() => {
    if (document.fonts.status === 'loaded') {
      setFontsLoaded(true);
    } else {
      document.fonts.ready.then(() => setFontsLoaded(true));
    }
  }, []);

  useGSAP(
    () => {
      if (!ref.current || !text || !fontsLoaded) return;
      
      const chars = ref.current.querySelectorAll('.split-char');
      
      gsap.fromTo(
        chars,
        { ...from },
        {
          ...to,
          duration,
          ease,
          stagger: delay / 1000,
          scrollTrigger: {
            trigger: ref.current,
            start: `top ${threshold * 100}%`,
            once: true,
          }
        }
      );
    },
    { dependencies: [text, fontsLoaded], scope: ref }
  );

  const Tag = tag;

  return (
    <Tag 
      ref={ref} 
      className={className} 
      style={{ textAlign, display: 'inline-block', overflow: 'hidden' }}
    >
      {text.split('').map((char, i) => (
        <span 
          key={i} 
          className="split-char" 
          style={{ display: 'inline-block', whiteSpace: char === ' ' ? 'pre' : 'normal' }}
        >
          {char}
        </span>
      ))}
    </Tag>
  );
};

export default SplitText;
